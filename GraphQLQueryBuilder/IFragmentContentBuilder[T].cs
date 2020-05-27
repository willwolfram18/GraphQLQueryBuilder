using System;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder
{
    public interface IFragmentContentBuilder<T> : IFragmentContentBuilder
        where T : class
    {
        new IFragmentContentBuilder<T> AddField(string field);

        new IFragmentContentBuilder<T> AddField(string alias, string field);

        IFragmentContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression);

        IFragmentContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression);

        IFragmentContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet)
            where TProperty : class;

        IFragmentContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet)
            where TProperty : class;
    }
}