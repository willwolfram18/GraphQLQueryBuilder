using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder
{
    public static class QueryContentBuilderExtensions
    {
        public static IGraphQLQueryContentBuilder<T> AddField<T, TProperty>(
            this IGraphQLQueryContentBuilder<T> builder,
            Expression<Func<T, TProperty>> expression,
            Action<IGraphQLQueryContentBuilder<TProperty>> configureSelectionSet)
            where T : class
            where TProperty : class
        {
            throw new NotImplementedException();
        }

        //public static QueryBuilder<T> AddField<T, TProperty>(
        //    this QueryBuilder<T> builder,
        //    string alias,
        //    Expression<Func<T, TProperty>> expression,
        //    Action<QueryBuilder<TProperty>> configureSelectionSet)
        //    where T : class
        //    where TProperty : class
        //{
        //    if (expression.Body is MemberExpression member)
        //    {
        //        var propertyQuery = new QueryBuilder<TProperty>(member.Member.Name);

        //        configureSelectionSet(propertyQuery);

        //        builder.AddQuery(alias, propertyQuery);

        //        return builder;
        //    }

        //    throw new InvalidOperationException();
        //}

        //public static QueryBuilder<T> AddCollectionField<T, TProperty>(
        //    this QueryBuilder<T> builder,
        //    Expression<Func<T, IEnumerable<TProperty>>> expression,
        //    Action<QueryBuilder<TProperty>> configureSelectionSet)
        //    where T : class
        //    where TProperty : class
        //{
        //    return builder.AddCollectionField(string.Empty, expression, configureSelectionSet);
        //}

        //public static QueryBuilder<T> AddCollectionField<T, TProperty>(
        //    this QueryBuilder<T> builder,
        //    string alias,
        //    Expression<Func<T, IEnumerable<TProperty>>> expression,
        //    Action<QueryBuilder<TProperty>> configureSelectionSet)
        //    where T : class
        //    where TProperty : class
        //{
        //    if (expression.Body is MemberExpression member)
        //    {
        //        var propertyQuery = new QueryBuilder<TProperty>(member.Member.Name);

        //        configureSelectionSet(propertyQuery);

        //        builder.AddQuery(alias, propertyQuery);

        //        return builder;
        //    }

        //    throw new InvalidOperationException();
        //}
    }
}