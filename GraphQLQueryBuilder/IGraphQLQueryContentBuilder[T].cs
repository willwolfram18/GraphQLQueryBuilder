using System;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder
{
    public interface IGraphQLQueryContentBuilder<T> : IGraphQLQueryContentBuilder
        where T : class
    {
        IGraphQLQueryContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression);

        IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression);

        IGraphQLQueryContentBuilder<T> AddFragment(IFragmentContentBuilder<T> fragment);
    }
}