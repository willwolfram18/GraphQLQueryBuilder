using System;
using System.Text;

namespace GraphQLQueryBuilder
{
    public class QueryRootBuilder : QueryBuilder
    {
        public QueryRootBuilder() : base("query")
        {
        }

        public QueryRootBuilder AddQuery(QueryBuilder query)
        {
            AddChildQuery(query);

            return this;
        }

        public QueryRootWithOperationNameBuilder AddOperationName(string name)
        {
            return new QueryRootWithOperationNameBuilder(name, ChildQueries);
        }

        public QueryRootBuilder AddQuery(string alias, QueryBuilder query)
        {
            AddChildQuery(alias, query);

            return this;
        }

        public QueryRootBuilder AddFragment(FragmentBuilder fragment)
        {
            AddFragmentDefinition(fragment);

            return this;
        }

        internal override string Build(uint indentationLevel)
        {
            var queryAppender = new QueryContentAppender(QueryName, indentationLevel);

            foreach (var query in ChildQueries)
            {
                queryAppender.AppendChildQuery(query.Alias, query.Query);
            }

            foreach (var fragment in FragmentDefinitions.Values)
            {
                queryAppender.AddFragmentDefinition(fragment);
            }

            return queryAppender.ToString();
        }
    }
}