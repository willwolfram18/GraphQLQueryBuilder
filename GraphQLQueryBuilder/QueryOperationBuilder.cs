using System;

namespace GraphQLQueryBuilder
{
    public class QueryOperationBuilder : IGraphQLQueryContentBuilder, IQueryOperation
    {
        public QueryOperationBuilder()
        {
        }

        public QueryOperationBuilder(string operationName)
        {
            throw new NotImplementedException();
        }

        public string OperationName { get; }

        public GraphQLOperationTypes OperationType => GraphQLOperationTypes.Query;

        public IGraphQLQueryContentBuilder AddField(string field)
        {
            throw new NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field)
        {
            throw new NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string field, ISelectionSet selectionSet)
        {
            throw new NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field, ISelectionSet selectionSet)
        {
            throw new NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddFragment(IFragmentContentBuilder fragment)
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            throw new NotImplementedException();
        }
    }
}