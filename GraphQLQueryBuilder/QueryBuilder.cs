using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GraphQLQueryBuilder
{
    public abstract class QueryBuilder
    {
        protected const char IndentCharacter = ' ';
        protected const int IndentWidth = 4;

        protected QueryBuilder()
        {
        }

        protected QueryBuilder(string name)
        {
            QueryName = name;
        }

        protected List<QueryBuilder> ChildQueries { get; } = new List<QueryBuilder>();

        protected string QueryName { get; }

        protected abstract string Build(uint indentationLevel);

        protected string BuildChildQueries(uint indentationLevel)
        {
            var query = new StringBuilder();

            foreach (var childQuery in ChildQueries)
            {
                query.Append(childQuery.Build(indentationLevel + 1));
            }

            return query.ToString();
        }

        protected string IndentationString(uint indentationLevel)
        {
            return new string(IndentCharacter, (int)indentationLevel * IndentWidth);
        }
    }
}