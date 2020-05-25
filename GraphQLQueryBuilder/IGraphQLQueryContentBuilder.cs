namespace GraphQLQueryBuilder
{
    public interface IGraphQLQueryContentBuilder : ISelectionSet
    {
        IGraphQLQueryContentBuilder AddField(string field);

        IGraphQLQueryContentBuilder AddField(string alias, string field);

        IGraphQLQueryContentBuilder AddField(string field, ISelectionSet selectionSet);

        IGraphQLQueryContentBuilder AddField(string alias, string field, ISelectionSet selectionSet);

        IGraphQLQueryContentBuilder AddFragment(IFragmentContentBuilder fragment);
    }
}