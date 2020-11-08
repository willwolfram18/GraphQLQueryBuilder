namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IStringArgumentValue : IArgumentValue, ILiteralArgumentValue
    {
        string Value { get; }
    }
}