namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IArgument
    {
        string Name { get; }
        
        IArgumentValue Value { get; }
    }
}