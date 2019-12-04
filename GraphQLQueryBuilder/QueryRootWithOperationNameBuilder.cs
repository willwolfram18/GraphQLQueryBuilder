using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLQueryBuilder
{
    public class QueryRootWithOperationNameBuilder : QueryBuilder
    {
        public QueryRootWithOperationNameBuilder(string operationName)
        {
            OperationName = operationName;
        }

        internal QueryRootWithOperationNameBuilder(string operationName, IEnumerable<ChildQuery> childQueries) : this(operationName)
        {
            foreach (var childQuery in childQueries)
            {
                AddChildQuery(childQuery.Alias, childQuery.Query);
            }
        }

        public string OperationName { get; }

        public QueryRootWithOperationNameBuilder AddQuery(QueryBuilder query)
        {
            AddChildQuery(query);

            return this;
        }

        public QueryRootWithOperationNameBuilder AddQuery(string alias, QueryBuilder query)
        {
            AddChildQuery(alias, query);

            return this;
        }

        internal override string Build(uint indentationLevel)
        {
            var queryWithOperationName = CreateQueryOperation();
            var queryAppender = new QueryContentAppender(queryWithOperationName, indentationLevel);

            foreach (var childQuery in ChildQueries)
            {
                queryAppender.AppendChildQuery(childQuery.Alias, childQuery.Query);
            }

            return queryAppender.ToString();
        }

        private string CreateQueryOperation()
        {
            return $"query {OperationName}";
        }
    }
}
