using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GraphQLQueryBuilder
{
    public abstract class QueryBuilder
    {
        protected QueryBuilder()
        {
        }

        protected QueryBuilder(string name)
        {
            QueryName = name;
        }

        protected List<QueryBuilder> ChildQueries { get; } = new List<QueryBuilder>();

        public string QueryName { get; }

        protected void AddChildQuery(QueryBuilder query)
        {
            ChildQueries.Add(query);
        }

        internal abstract string Build(uint indentationLevel);
    }
}