namespace GraphQLQueryBuilder
{
    public interface IGraphQLQueryContentBuilder : ISelectionSet
    {
        IGraphQLQueryContentBuilder AddField(string field);

        IGraphQLQueryContentBuilder AddField(string alias, string field);

        IGraphQLQueryContentBuilder AddFragment(IFragmentContentBuilder fragment);

        IGraphQLQueryContentBuilder AddSelectionSet(ISelectionSet selectionSet);
    }
}