namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IFieldSelectionItem : ISelectionSetItem
    {
        string Alias { get; }

        string FieldName { get; }

        ISelectionSet SelectionSet { get; }
    }
}