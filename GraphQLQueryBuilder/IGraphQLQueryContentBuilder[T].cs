using System;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder
{
    public interface IGraphQLQueryContentBuilder<T> : IGraphQLQueryContentBuilder, ISelectionSet<T>
        where T : class
    {
        IGraphQLQueryContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression);

        IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression);

        IGraphQLQueryContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet)
            where TProperty : class;

        IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet)
            where TProperty : class;
    }
}