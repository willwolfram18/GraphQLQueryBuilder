namespace GraphQLQueryBuilder
{
    public interface IFragmentContentBuilder : ISelectionSet
    {
        string Name { get; }

        string TypeCondition { get; }

        IFragmentContentBuilder AddField(string field);

        IFragmentContentBuilder AddField(string alias, string field);

        IFragmentContentBuilder AddSelectionSet(ISelectionSet fieldSelection);
    }
}