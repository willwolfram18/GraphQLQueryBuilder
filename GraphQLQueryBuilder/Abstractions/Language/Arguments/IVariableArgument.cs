namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IVariableArgument : IArgument
    {
        IVariable Variable { get; }
    }
}