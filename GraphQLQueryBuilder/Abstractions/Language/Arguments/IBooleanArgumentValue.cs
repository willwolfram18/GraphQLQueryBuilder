namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IBooleanArgumentValue : IArgumentValue
    {
        bool Value { get; }
    }
}