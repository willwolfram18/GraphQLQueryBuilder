namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IBooleanArgumentValue : IArgumentValue, ILiteralArgumentValue
    {
        bool Value { get; }
    }
}