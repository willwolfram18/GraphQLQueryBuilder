using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using GraphQLQueryBuilder.Implementations.Language;

namespace GraphQLQueryBuilder
{
    public static class SelectionSetBuilder
    {
        public static ISelectionSetBuilder<T> Of<T>() where T : class
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

            if (!string.IsNullOrWhiteSpace(alias) && !GraphQLName.IsValid(alias))
            {
                throw new ArgumentException("Provided alias does not comply with GraphQL's Name specification.", nameof(alias))
                {
                    HelpLink = "https://spec.graphql.org/June2018/#Name"
                };
            }

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
                throw new InvalidOperationException($"Property '{memberExpression.Member.Name}' is not a member of type '{typeof(T).FullName}'.");
            }

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
            throw new System.NotImplementedException();
            return this;
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddCollectionField<TProperty>(Expression<Func<T, System.Collections.Generic.IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        /// <inheritdoc />
        public ISelectionSetBuilder<T> AddCollectionField<TProperty>(string alias, Expression<Func<T, System.Collections.Generic.IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        /// <inheritdoc />
        public ISelectionSet<T> Build()
        {
            if (_selectionSetItems.Count == 0)
            {
                throw new InvalidOperationException("A selection set must include one or more fields.")
                {
                    HelpLink = "https://spec.graphql.org/June2018/#SelectionSet"
                };
            }

            return new SelectionSet<T>(_selectionSetItems);
        }
    }
}