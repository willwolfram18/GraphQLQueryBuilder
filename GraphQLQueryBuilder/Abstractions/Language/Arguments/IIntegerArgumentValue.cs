namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IIntegerArgumentValue : IArgumentValue, ILiteralArgumentValue
    {
        int Value { get; }
    }
}