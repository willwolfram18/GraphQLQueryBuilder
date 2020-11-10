namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IGraphQLOperation
    {
        GraphQLOperationType Type { get; }

        string Name { get; }

        ISelectionSet SelectionSet { get; }
    }
}