using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Guards;
using GraphQLQueryBuilder.Implementations.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GraphQLQueryBuilder
{
    public static class SelectionSetBuilder
    {
        public static ISelectionSetBuilder<T> For<T>() where T : class
        {
            return new SelectionSetBuilder<T>();
        }
    }

    internal class SelectionSetBuilder<T> : ISelectionSetBuilder<T> where T : class
    {
        private readonly PropertyInfo[] _properties = typeof(T).GetProperties();
        private readonly List<ISelectionSetItem> _selectionSetItems = new List<ISelectionSetItem>();

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddScalarField<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return AddScalarField(null, expression);
        }

        public ISelectionSetBuilder<T> AddScalarField<TProperty>(Expression<Func<T, TProperty>> expression, IEnumerable<IArgument> arguments)
        {
            return AddScalarField(null, expression, arguments);
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddScalarField<TProperty>(string alias, Expression<Func<T, TProperty>> expression)
        {
            return AddScalarField(alias, expression, null);
        }

        public ISelectionSetBuilder<T> AddScalarField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, IEnumerable<IArgument> arguments)
        {
            alias = alias.MustBeValidGraphQLName(nameof(alias));
            
            typeof(TProperty).MustBeGraphQLScalar();

            var propertyInfo = GetPropertyInfoForExpression(expression);
            var args = (arguments ?? Enumerable.Empty<IArgument>()).Where(arg => arg != null).ToList();

            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, args, null));

            return this;
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddObjectField<TProperty>(Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            return AddObjectField(null, expression, selectionSet);
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddObjectField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            alias = alias.MustBeValidGraphQLName(nameof(alias));

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            var propertyInfo = GetPropertyInfoForExpression(expression);

            typeof(TProperty).MustBeAGraphQLObject($" Use the {nameof(AddScalarField)} method.")
                .MustNotBeGraphQLList($" Use the {nameof(AddScalarCollectionField)} or {nameof(AddObjectCollectionField)} methods.");

            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, selectionSet));

            return this;
        }

        public ISelectionSetBuilder<T> AddScalarCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            return AddScalarCollectionField(null, expression);
        }

        public ISelectionSetBuilder<T> AddScalarCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            alias = alias.MustBeValidGraphQLName(nameof(alias));
            
            typeof(TProperty).MustBeGraphQLScalar();
            
            var propertyInfo = GetPropertyInfoForExpression(expression);

            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, null));

            return this;
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddObjectCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            return AddObjectCollectionField(null, expression, selectionSet);
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddObjectCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            alias = alias.MustBeValidGraphQLName(nameof(alias));

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            var propertyInfo = GetPropertyInfoForExpression(expression);
            
            typeof(TProperty).MustBeAGraphQLObject($" Use the {nameof(AddScalarCollectionField)} method.");
            
            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, selectionSet));
            
            return this;
        }

        /// <inheritdoc />
        public ISelectionSet<T> Build()
        {
            return new SelectionSet<T>(_selectionSetItems);
        }

        private PropertyInfo GetPropertyInfoForExpression<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!(expression.Body is MemberExpression memberExpression))
            {
                throw new ArgumentException($"A {nameof(MemberExpression)} was not provided.", nameof(expression));
            }

            var propertyInfo = _properties.SingleOrDefault(property => property == memberExpression.Member);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException(
                    $"Property '{memberExpression.Member.Name}' is not a member of type '{typeof(T).FullName}'.");
            }

            return propertyInfo;
        }
    }
}