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

        protected override string Build(uint indentationLevel)
        {
            var indentation = IndentationString(indentationLevel);
            var query = new StringBuilder()
                .AppendLine(indentation + QueryName + " {");

            var childQueries = BuildChildQueries(indentationLevel);

            if (!string.IsNullOrWhiteSpace(childQueries))
            {
                query.Append(childQueries);
            }

            query.AppendLine(indentation + "}");

            return query.ToString();
        }
    }
}