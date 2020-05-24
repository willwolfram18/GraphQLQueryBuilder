namespace GraphQLQueryBuilder
{
    public interface IFragmentContentBuilder : ISelectionSet
    {
        string Name { get; }

        string Type { get; }

        IFragmentContentBuilder AddField(string field);

        IFragmentContentBuilder AddField(string alias, string field);

        IFragmentContentBuilder AddFragment(IFragmentContentBuilder fragment);

        IFragmentContentBuilder AddSelectionSet(ISelectionSet fieldSelection);

        string Build();
    }
}