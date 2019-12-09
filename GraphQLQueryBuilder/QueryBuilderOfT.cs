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

        public QueryBuilder<T> AddQuery(string alias, QueryBuilder query)
        {
            AddChildQuery(alias, query);

            return this;
        }

        public QueryBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression)
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

        public QueryRootBuilder AddFragment(FragmentBuilder fragment)
        {
            throw new NotImplementedException();
        }

        internal override string Build(uint indentationLevel)
        {
            var queryAppender = new QueryContentAppender(QueryName, indentationLevel);

            foreach (var property in _properties)
            {
                queryAppender.AppendProperty(property);
            }

            foreach (var query in ChildQueries)
            {
                queryAppender.AppendChildQuery(query.Alias, query.Query);
            }

            return queryAppender.ToString();
        }
    }
}