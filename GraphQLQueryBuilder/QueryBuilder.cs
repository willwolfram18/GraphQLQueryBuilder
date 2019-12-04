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

        protected List<ChildQuery> ChildQueries { get; } = new List<ChildQuery>();

        public string QueryName { get; }

        public string Build()
        {
            return Build(0);
        }

        protected void AddChildQuery(QueryBuilder query)
        {
            ChildQueries.Add(new ChildQuery(query));
        }

        protected void AddChildQuery(string alias, QueryBuilder query)
        {
            ChildQueries.Add(new ChildQuery(alias, query));
        }

        internal abstract string Build(uint indentationLevel);
    }
}