using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder
{
    public static class SelectionSetBuilderExtensions
    {
        public static ISelectionSetBuilder<T> AddScalarField<T, TProperty>(this ISelectionSetBuilder<T> builder,
            Expression<Func<T, TProperty>> expression)
            where T : class
        {
            return builder.AddScalarField(null, expression);
        }

        public static ISelectionSetBuilder<T> AddScalarField<T, TProperty>(this ISelectionSetBuilder<T> builder,
            Expression<Func<T, TProperty>> expression, IArgumentCollection arguments)
            where T : class
        {
            return builder.AddScalarField(null, expression, arguments);
        }

        public static ISelectionSetBuilder<T> AddScalarField<T, TProperty>(this ISelectionSetBuilder<T> builder,
            string alias,
            Expression<Func<T, TProperty>> expression)
            where T : class
        {
            return builder.AddScalarField(alias, expression, null);
        }

        public static ISelectionSetBuilder<T> AddScalarCollectionField<T, TProperty>(
            this ISelectionSetBuilder<T> builder,
            Expression<Func<T, IEnumerable<TProperty>>> expression)
            where T : class
        {
            return builder.AddScalarCollectionField(null, expression);
        }

        public static ISelectionSetBuilder<T> AddScalarCollectionField<T, TProperty>(
            this ISelectionSetBuilder<T> builder,
            string alias,
            Expression<Func<T, IEnumerable<TProperty>>> expression)
            where T : class
        {
            return builder.AddScalarCollectionField(alias, expression, null);
        }

        public static ISelectionSetBuilder<T> AddScalarCollectionField<T, TProperty>(
            this ISelectionSetBuilder<T> builder,
            Expression<Func<T, IEnumerable<TProperty>>> expression,
            IArgumentCollection arguments)
            where T : class
        {
            return builder.AddScalarCollectionField(null, expression, arguments);
        }

        public static ISelectionSetBuilder<T> AddObjectField<T, TProperty>(this ISelectionSetBuilder<T> builder,
            Expression<Func<T, TProperty>> expression,
            ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectField(null, expression, selectionSet);
        }

        public static ISelectionSetBuilder<T> AddObjectField<T, TProperty>(this ISelectionSetBuilder<T> builder,
            Expression<Func<T, TProperty>> expression,
            IArgumentCollection arguments, ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectField(null, expression, arguments, selectionSet);
        }

        public static ISelectionSetBuilder<T> AddObjectField<T, TProperty>(this ISelectionSetBuilder<T> builder,
            string alias,
            Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectField(alias, expression, null, selectionSet);
        }

        public static ISelectionSetBuilder<T> AddObjectCollectionField<T, TProperty>(
            this ISelectionSetBuilder<T> builder,
            Expression<Func<T, IEnumerable<TProperty>>> expression,
            ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectCollectionField(null, expression, null, selectionSet);
        }

        public static ISelectionSetBuilder<T> AddObjectCollectionField<T, TProperty>(
            this ISelectionSetBuilder<T> builder,
            string alias,
            Expression<Func<T, IEnumerable<TProperty>>> expression,
            ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectCollectionField(alias, expression, null, selectionSet);
        }

        public static ISelectionSetBuilder<T> AddObjectCollectionField<T, TProperty>(
            this ISelectionSetBuilder<T> builder,
            Expression<Func<T, IEnumerable<TProperty>>> expression, IArgumentCollection arguments,
            ISelectionSet<TProperty> selectionSet)
            where T : class
            where TProperty : class
        {
            return builder.AddObjectCollectionField(null, expression, arguments, selectionSet);
        }
    }
}