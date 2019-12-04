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

        public QueryRootBuilder AddQuery(string alias, QueryBuilder query)
        {
            AddChildQuery(alias, query);

            return this;
        }

        public string Build()
        {
            return Build(0);
        }

        internal override string Build(uint indentationLevel)
        {
            var queryAppender = new QueryContentAppender(QueryName, indentationLevel);

            foreach (var query in ChildQueries)
            {
                queryAppender.AppendChildQuery(query.Alias, query.Query);
            }

            return queryAppender.Build();
        }
    }
}