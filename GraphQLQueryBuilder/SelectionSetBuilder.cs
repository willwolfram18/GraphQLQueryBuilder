using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Implementations.Language;
using System;
using System.Collections;
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
        public ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return AddField(null, expression);
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> expression)
        {
            alias = alias?.Trim();

            ThrowIfAliasIsNotValid(alias);

            var propertyInfo = GetPropertyInfoForExpression(expression);
            if (propertyInfo.PropertyType.IsClass && propertyInfo.PropertyType != typeof(string))
            {
                throw new InvalidOperationException(
                    $"When selecting a property that is a class, please use the {nameof(AddCollectionField)} method that takes an {nameof(ISelectionSet)}."
                );
            }

            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, null));

            return this;
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            return AddField(null, expression, selectionSet);
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            alias = alias?.Trim();

            ThrowIfAliasIsNotValid(alias);

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            var propertyInfo = GetPropertyInfoForExpression(expression);
            if (propertyInfo.PropertyType == typeof(string))
            {
                throw new InvalidOperationException(
                    $"When selecting a property that is of type string, please use the {nameof(AddField)} method that does not take an {nameof(ISelectionSet)}."
                );
            }
            if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
            {
                throw new InvalidOperationException(
                    $"When selecting a property that is an {nameof(IEnumerable)}, please use the {nameof(AddCollectionField)} method."
                );
            }

            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, selectionSet));

            return this;
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            return AddCollectionField(null, expression, selectionSet);
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            alias = alias?.Trim();

            ThrowIfAliasIsNotValid(alias);

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            var propertyInfo = GetPropertyInfoForExpression(expression);
            
            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, selectionSet));
            
            return this;
        }

        /// <inheritdoc />
        public ISelectionSet<T> Build()
        {
            return new SelectionSet<T>(_selectionSetItems);
        }

        private static void ThrowIfAliasIsNotValid(string alias)
        {
            if (!string.IsNullOrWhiteSpace(alias) && !GraphQLName.IsValid(alias))
            {
                throw new ArgumentException("Provided alias does not comply with GraphQL's Name specification.", nameof(alias))
                {
                    HelpLink = "https://spec.graphql.org/June2018/#Name"
                };
            }
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