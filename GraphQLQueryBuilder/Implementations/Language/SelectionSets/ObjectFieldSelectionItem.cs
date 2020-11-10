using System.Collections.Generic;
using System.Linq;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class ObjectFieldSelectionItem : IFieldSelectionItem
    {
        public ObjectFieldSelectionItem(string alias, string fieldName, ISelectionSet selectionSet) : this(alias, fieldName, null, selectionSet)
        {
        }

        public ObjectFieldSelectionItem(string alias, string fieldName, IEnumerable<IArgument> arguments,
            ISelectionSet selectionSet)
        {
            Alias = alias;
            FieldName = fieldName;
            Arguments = (arguments ?? Enumerable.Empty<IArgument>())
                .Where(arg => arg != null)
                .ToList()
                .AsReadOnly();
            SelectionSet = selectionSet;
        }

        /// <inheritdoc />
        public string Alias { get; }

        /// <inheritdoc />
        public string FieldName { get; }
        
        /// <inheritdoc />
        public IReadOnlyCollection<IArgument> Arguments { get; }

        /// <inheritdoc />
        public ISelectionSet SelectionSet { get; }
    }
}
