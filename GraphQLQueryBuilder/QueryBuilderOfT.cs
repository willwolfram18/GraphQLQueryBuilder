using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GraphQLQueryBuilder
{
    public class QueryBuilder<T> where T : class
    {
        private readonly List<string> _properties = new List<string>();

        public QueryBuilder<T> AddQuery(string queryName) {
            // throw new NotImplementedException();
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

        public string Build()
        {
            var query = new StringBuilder()
                .AppendLine("query {")
                .AppendLine("    address {");

            var properties = string.Join(
                $",{Environment.NewLine}",
                _properties.Select(p => $"        {p}")
            );

            query.AppendLine(properties)
                .AppendLine("    }")
                .AppendLine("}");

            return query.ToString();
        }
    }
}