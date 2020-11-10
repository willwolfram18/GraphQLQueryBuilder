namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IGraphQLOperation<T> : IGraphQLOperation where T : class
    {
        new ISelectionSet<T> SelectionSet { get; }
    }
}
