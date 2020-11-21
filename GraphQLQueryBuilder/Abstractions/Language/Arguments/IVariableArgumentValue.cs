namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IVariableArgumentValue : IArgumentValue
    {
        IVariable Value { get; }
    }
}