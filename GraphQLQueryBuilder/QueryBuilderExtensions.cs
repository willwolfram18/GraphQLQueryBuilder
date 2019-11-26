using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder
{
    public static class QueryBuilderExtensions
    {
        public static QueryBuilder<T> AddProperty<T, TProperty>(
            this QueryBuilder<T> builder,
            Expression<Func<T, TProperty>> expression,
            Action<QueryBuilder<TProperty>> configurePropertyQuery)
            where T : class
            where TProperty : class
        {
            if (expression.Body is MemberExpression member)
            {
                var propertyQuery = new QueryBuilder<TProperty>(member.Member.Name);

                configurePropertyQuery(propertyQuery);

                builder.AddQuery(propertyQuery);

                return builder;
            }

            throw new InvalidOperationException();
        }

        public static QueryBuilder<T> AddCollectionProperty<T, TProperty>(
            this QueryBuilder<T> builder,
            Expression<Func<T, IEnumerable<TProperty>>> expression,
            Action<QueryBuilder<TProperty>> configurePropertyQuery)
            where T : class
            where TProperty : class
        {
            if (expression.Body is MemberExpression member)
            {
                var propertyQuery = new QueryBuilder<TProperty>(member.Member.Name);

                configurePropertyQuery(propertyQuery);

                builder.AddQuery(propertyQuery);

                return builder;
            }

            throw new InvalidOperationException();
        }
    }
}