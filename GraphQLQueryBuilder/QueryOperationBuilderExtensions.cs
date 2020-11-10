using System;
using System.Collections.Generic;
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

        public static IQueryOperationBuilder<T> AddObjectField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            Expression<Func<T, TProperty>> expression,
            ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectField(null, expression, selectionSet);
        }

        public static IQueryOperationBuilder<T> AddObjectField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            Expression<Func<T, TProperty>> expression, IArgumentCollection arguments,
            ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectField(null, expression, arguments, selectionSet);
        }

        public static IQueryOperationBuilder<T> AddObjectField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            string alias, Expression<Func<T, TProperty>> expression,
            ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectField(alias, expression, null, selectionSet);
        }

        public static IQueryOperationBuilder<T> AddScalarCollectionField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            Expression<Func<T, IEnumerable<TProperty>>> expression)
            where T : class
        {
            return builder.AddScalarCollectionField(null, expression);
        }

        public static IQueryOperationBuilder<T> AddScalarCollectionField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            string alias,
            Expression<Func<T, IEnumerable<TProperty>>> expression)
            where T : class
        {
            return builder.AddScalarCollectionField(alias, expression, null);
        }

        public static IQueryOperationBuilder<T> AddScalarCollectionField<T, TProperty>(this IQueryOperationBuilder<T> builder,
            Expression<Func<T, IEnumerable<TProperty>>> expression,
            IArgumentCollection arguments)
            where T : class
        {
            return builder.AddScalarCollectionField(null, expression, arguments);
        }
    }
}