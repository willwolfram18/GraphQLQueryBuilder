namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IIntegerArgumentValue : IArgumentValue
    {
        int Value { get; }
    }
}