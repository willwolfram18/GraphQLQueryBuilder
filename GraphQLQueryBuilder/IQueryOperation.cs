namespace GraphQLQueryBuilder
{
    public interface IQueryOperation
    {
        GraphQLOperationTypes OperationType { get; }
    }
}