namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IFloatArgumentValue : IArgumentValue
    {
        double Value { get; }
    }
}