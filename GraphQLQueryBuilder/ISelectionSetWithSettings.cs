namespace GraphQLQueryBuilder
{
    internal interface ISelectionSetWithSettings : ISelectionSet
    {
        ISelectionSetWithSettings UpdateSettings(QuerySerializerSettings settings);
    }
}