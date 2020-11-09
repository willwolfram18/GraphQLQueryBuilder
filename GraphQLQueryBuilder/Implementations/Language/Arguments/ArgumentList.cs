using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class ArgumentList : IArgumentList
    {
        private readonly List<IArgument> _arguments = new List<IArgument>();
        private readonly HashSet<string> _argumentNames = new HashSet<string>();

        /// <inheritdoc />
        public IEnumerator<IArgument> GetEnumerator() => _arguments.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc />
        public void Add(IArgument item)
        {
            if (Contains(item ?? throw new ArgumentNullException(nameof(item))))
            {
                throw new InvalidOperationException($"There can be only one argument named '{item.Name}'.");
            }

            _arguments.Add(item);
            _argumentNames.Add(item.Name);
        }

        /// <inheritdoc />
        public void Clear()
        {
            _arguments.Clear();
            _argumentNames.Clear();
        }

        /// <inheritdoc />
        public bool Contains(IArgument item)
        {
            return _argumentNames.Contains(item?.Name ?? throw new ArgumentNullException(nameof(item)));
        }

        /// <inheritdoc />
        public void CopyTo(IArgument[] array, int arrayIndex) => _arguments.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(IArgument item)
        {
            if (!Contains(item ?? throw new ArgumentNullException(nameof(item))))
            {
                return false;
            }

            var itemToRemove = _arguments.First(arg => arg.Name == item.Name);

            return _arguments.Remove(itemToRemove) && _argumentNames.Remove(item.Name);
        }

        /// <inheritdoc />
        public int Count => _arguments.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;
    }
}