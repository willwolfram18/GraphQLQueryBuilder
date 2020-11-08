namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IFloatArgumentValue : IArgumentValue, ILiteralArgumentValue
    {
        double Value { get; }
    }
}