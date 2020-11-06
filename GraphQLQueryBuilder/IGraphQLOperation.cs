namespace GraphQLQueryBuilder
{
    public interface IGraphQLOperation
    {
        GraphQLOperationType Type { get; }

        string Name { get; }

        ISelectionSet SelectionSet { get; }
    }
}