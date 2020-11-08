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
            Expression<Func<T, TProperty>> expression, IEnumerable<IArgument> arguments)
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
            Expression<Func<T, IEnumerable<TProperty>>> expression,
            IEnumerable<IArgument> arguments)
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
            IEnumerable<IArgument> arguments, ISelectionSet<TProperty> selectionSet)
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
    }
}