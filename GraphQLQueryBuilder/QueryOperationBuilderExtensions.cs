using System;
using System.Linq.Expressions;
using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder
{
    public static class QueryOperationBuilderExtensions
    {
        public static IQueryOperationBuilder<T> AddScalarField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            Expression<Func<T, TProperty>> expression)
            where T : class
        {
            return builder.AddScalarField(null, expression);
        }
        
        public static IQueryOperationBuilder<T> AddScalarField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            string alias, Expression<Func<T, TProperty>> expression)
            where T : class
        {
            return builder.AddScalarField(alias, expression, null);
        }
        
        public static IQueryOperationBuilder<T> AddScalarField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            Expression<Func<T, TProperty>> expression, IArgumentCollection arguments)
            where T : class
        {
            return builder.AddScalarField(null, expression, arguments);
        }
    }
}