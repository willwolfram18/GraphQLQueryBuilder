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

        public string Build()
        {
            return Build(0);
        }

        internal override string Build(uint indentationLevel)
        {
            var queryAppender = new QueryAppender(QueryName, indentationLevel);

            foreach (var query in ChildQueries)
            {
                queryAppender.AppendChildQuery(query);
            }

            return queryAppender.Build();
        }
    }
}