using System.Collections.Generic;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class FieldSelectionItem : IFieldSelectionItem
    {
        public FieldSelectionItem(string alias, string fieldName, ISelectionSet selectionSet) : this(alias, fieldName, null, selectionSet)
        {
        }

        public FieldSelectionItem(string alias, string fieldName, IEnumerable<IArgument> arguments,
            ISelectionSet selectionSet)
        {
            Alias = alias;
            FieldName = fieldName;
            Arguments = arguments;
            SelectionSet = selectionSet;
        }

        /// <inheritdoc />
        public string Alias { get; }

        /// <inheritdoc />
        public string FieldName { get; }
        
        /// <inheritdoc />
        public IEnumerable<IArgument> Arguments { get; }

        /// <inheritdoc />
        public ISelectionSet SelectionSet { get; }
    }
}
