using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class ArgumentCollection : IArgumentCollection
    {
        private readonly OrderedDictionary _args = new OrderedDictionary();

        public ArgumentCollection()
        {
        }

        public ArgumentCollection(IEnumerable<IArgument> arguments)
        {
            AddRange(arguments);
        }

        /// <inheritdoc />
        IEnumerator<KeyValuePair<string, IArgumentValue>> IEnumerable<KeyValuePair<string, IArgumentValue>>.
            GetEnumerator()
        {
            return _args.Keys.Cast<string>().Select(key => new KeyValuePair<string, IArgumentValue>(key, this[key]))
                .GetEnumerator();
        }

        /// <inheritdoc />
        public IEnumerator<IArgument> GetEnumerator() => _args.Values.Cast<IArgument>().GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(IArgument item)
        {
            if (Contains(item ?? throw new ArgumentNullException(nameof(item))))
            {
                throw new InvalidOperationException($"There can be only one argument named '{item.Name}'.");
            }

            _args.Add(item.Name, item);
        }

        /// <inheritdoc />
        public void Clear() => _args.Clear();

        /// <inheritdoc />
        public bool Contains(IArgument item)
        {
            return _args.Contains(item?.Name ?? throw new ArgumentNullException(nameof(item)));
        }

        /// <inheritdoc />
        public void CopyTo(IArgument[] array, int arrayIndex) =>
            _args.Values.Cast<IArgument>().ToList().CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(IArgument item)
        {
            if (!Contains(item ?? throw new ArgumentNullException(nameof(item))))
            {
                return false;
            }

            _args.Remove(item.Name);

            return true;
        }

        /// <inheritdoc cref="ICollection{T}.Count" />
        public int Count => _args.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        /// <inheritdoc />
        public void AddRange(IEnumerable<IArgument> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var arg in collection)
            {
                Add(arg);
            }
        }

        /// <inheritdoc />
        public bool ContainsKey(string key) => _args.Contains(key);

        /// <inheritdoc />
        public bool TryGetValue(string key, out IArgumentValue value)
        {
            if (!ContainsKey(key))
            {
                value = default;
                return false;
            }

            value = this[key];
            return true;
        }

        /// <inheritdoc />
        public IArgumentValue this[string key] => ((IArgument) _args[key]).Value;

        /// <inheritdoc />
        public IEnumerable<string> Keys => _args.Keys.Cast<string>();

        /// <inheritdoc />
        public IEnumerable<IArgumentValue> Values => _args.Values.Cast<IArgument>().Select(arg => arg.Value);
    }
}