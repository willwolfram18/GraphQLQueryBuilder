using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class FieldSelectionItem : IFieldSelectionItem
    {
        public FieldSelectionItem(string alias, string fieldName, ISelectionSet selectionSet)
        {
            Alias = alias;
            FieldName = fieldName;
            SelectionSet = selectionSet;
        }

        /// <inheritdoc />
        public string Alias { get; }

        /// <inheritdoc />
        public string FieldName { get; }

        /// <inheritdoc />
        public ISelectionSet SelectionSet { get; }
    }
}
