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
        public ISelectionSetBuilder<T> AddScalarField<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return AddScalarField(null, expression);
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddScalarField<TProperty>(string alias, Expression<Func<T, TProperty>> expression)
        {
            alias = alias?.Trim();

            ThrowIfAliasIsNotValid(alias);

            var propertyType = typeof(TProperty);
            if (!GraphQLTypes.IsScalarType(propertyType))
            {
                throw new InvalidOperationException(
                    $"Type '{propertyType.FullName}' is not a GraphQL scalar type."
                );
            }

            var propertyInfo = GetPropertyInfoForExpression(expression);

            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, null));

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
            alias = alias?.Trim();

            ThrowIfAliasIsNotValid(alias);

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            var propertyInfo = GetPropertyInfoForExpression(expression);
            
            var propertyType = typeof(TProperty);
            if (GraphQLTypes.IsScalarType(propertyType))
            {
                throw new InvalidOperationException(
                    $"The type '{propertyType.FullName}' is a GraphQL scalar type so the {nameof(AddScalarField)} method should be used."
                )
                {
                    HelpLink = "http://spec.graphql.org/June2018/#sec-Scalars"
                };
            }
            if (GraphQLTypes.IsListType(propertyType))
            {
                throw new InvalidOperationException(
                    $"The type '{propertyType.FullName}' is a GraphQL list type so the {nameof(AddScalarCollectionField)} or {nameof(AddObjectCollectionField)} methods should be used."
                )
                {
                    HelpLink = "http://spec.graphql.org/June2018/#sec-Type-System.List"
                };
            }

            _selectionSetItems.Add(new FieldSelectionItem(alias, propertyInfo.Name, selectionSet));

            return this;
        }

        public ISelectionSetBuilder<T> AddScalarCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            return AddScalarCollectionField(null, expression);
        }

        public ISelectionSetBuilder<T> AddScalarCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddObjectCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            return AddObjectCollectionField(null, expression, selectionSet);
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddObjectCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            alias = alias?.Trim();

            ThrowIfAliasIsNotValid(alias);

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            var propertyInfo = GetPropertyInfoForExpression(expression);
            
            var propertyType = typeof(TProperty);
            if (!GraphQLTypes.IsObjectType(propertyType))
            {
                throw new InvalidOperationException($"The type '{propertyType.FullName}' is not a GraphQL object type.")
                {
                    HelpLink = "http://spec.graphql.org/June2018/#sec-Objects"
                };
            }
            
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