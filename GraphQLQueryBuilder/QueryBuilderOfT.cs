using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GraphQLQueryBuilder
{
    public class QueryBuilder<T> : QueryBuilder where T : class
    {
        private readonly List<string> _properties = new List<string>();

        public QueryBuilder(string name) : base(name)
        {
        }

        public QueryBuilder<T> AddQuery(QueryBuilder query)
        {
            AddChildQuery(query);

            return this;
        }

        public QueryBuilder<T> AddProperty<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (expression.Body is MemberExpression member)
            {
                _properties.Add(member.Member.Name);

                return this;
            }

            throw new InvalidOperationException("Not a member");
        }

        protected override string Build(uint indentationLevel)
        {
            var indentation = IndentationString(indentationLevel);
            var query = new StringBuilder()
                .AppendLine(indentation + QueryName + " {");

            var childQueries = BuildChildQueries(indentationLevel);
            var properties = BuildProperties(IndentationString(indentationLevel + 1));

            var propertiesIsEmpty = string.IsNullOrWhiteSpace(properties);
            var childQueriesAreEmpty = string.IsNullOrWhiteSpace(childQueries);

            if (!propertiesIsEmpty)
            {
                var propertiesToAppend = properties;
                if (!childQueriesAreEmpty)
                {
                    propertiesToAppend += ",";
                }
                query.AppendLine(propertiesToAppend);
            }
            if (!string.IsNullOrWhiteSpace(childQueries))
            {
                query.Append(childQueries);
            }

            query.AppendLine(indentation + "}");

            return query.ToString();
        }

        private string BuildProperties(string indentation)
        {
            var properties = string.Join(
                $",{Environment.NewLine}",
                _properties.Select(p => $"{indentation}{p}")
            );

            return properties;
        }
    }
}