using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLQueryBuilder
{
    public class QueryRootWithOperationNameBuilder : QueryBuilder
    {
        public QueryRootWithOperationNameBuilder(string operationName) : base($"query")
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

        internal override string Build(uint indentationLevel)
        {
            throw new NotImplementedException();
        }
    }
}
