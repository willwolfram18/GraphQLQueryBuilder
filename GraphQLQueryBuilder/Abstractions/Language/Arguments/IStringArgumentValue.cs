namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IStringArgumentValue : IArgumentValue
    {
        string Value { get; }
    }
}