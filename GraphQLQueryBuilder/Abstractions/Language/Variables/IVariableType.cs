namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IVariableType
    {
        string Name { get; }
        
        bool IsNullable { get; }
        
        bool IsList { get; }
    }
}