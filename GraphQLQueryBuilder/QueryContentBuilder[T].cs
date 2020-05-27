using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace GraphQLQueryBuilder
{
    public static class QueryContentBuilder
    {
        public static IGraphQLQueryContentBuilder<T> Of<T>()
            where T : class
        {
            return new QueryContentBuilder<T>();
        }
    }

    internal class QueryContentBuilder<T> : IGraphQLQueryContentBuilder<T>
        where T : class
    {
        private readonly PropertyInfo[] _properties;
        private static readonly Regex GraphQLNameRegex = new Regex(@"^[_A-Za-z][_0-9A-Za-z]*$");

        internal QueryContentBuilder()
        {
            _properties = typeof(T).GetProperties();
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var fieldName = ConvertExpressionToFieldName(propertyExpression);
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression)
        {
            ThrowIfInvalidName(alias, "The alias is invalid.", nameof(alias));

            var fieldName = ConvertExpressionToFieldName(propertyExpression);
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            var fieldName = ConvertExpressionToFieldName(propertyExpression);
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            ThrowIfInvalidName(alias, "The alias is invalid.", nameof(alias));

            var fieldName = ConvertExpressionToFieldName(propertyExpression);
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string field)
        {
            ThrowIfInvalidName(field, "The field name is invalid.", nameof(field));
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field)
        {
            ThrowIfInvalidName(alias, "The alias is invalid.", nameof(alias));
            ThrowIfInvalidName(field, "The field name is invalid.", nameof(field));
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string field, ISelectionSet selectionSet)
        {
            ThrowIfInvalidName(field, "The field name is invalid.", nameof(field));
            throw new System.NotImplementedException();
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field, ISelectionSet selectionSet)
        {
            ThrowIfInvalidName(alias, "The alias is invalid.", nameof(alias));
            ThrowIfInvalidName(field, "The field name is invalid.", nameof(field));
            throw new System.NotImplementedException();
        }

        public string Build()
        {
            return string.Empty;
        }

        private static void ThrowIfInvalidName(string value, string message, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value) || !GraphQLNameRegex.IsMatch(value))
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        private string ConvertExpressionToFieldName<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (propertyExpression.Body is MemberExpression memberExpression)
            {
                if (_properties.Any(property => property.Name == memberExpression.Member.Name))
                {
                    return memberExpression.Member.Name;
                }
            }

            throw new InvalidOperationException($"Expression is not a property of type {typeof(T).FullName}.");
        }
    }
}