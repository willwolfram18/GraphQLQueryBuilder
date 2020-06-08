using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;

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
        private static readonly Regex GraphQLNameRegex = new Regex(@"^[_A-Za-z][_0-9A-Za-z]*$");

        private readonly PropertyInfo[] _properties = typeof(T).GetProperties();
        private readonly List<ISelectionSet> _selections = new List<ISelectionSet>();

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            var fieldName = ConvertExpressionToFieldName(propertyExpression);

            return AddValidField(fieldName);
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression)
        {
            ThrowIfInvalidGraphQLName(alias, "The alias is invalid.", nameof(alias));

            var fieldName = ConvertExpressionToFieldName(propertyExpression);

            return AddValidField(alias, fieldName);
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            var fieldName = ConvertExpressionToFieldName(propertyExpression);

            return AddValidField(fieldName, selectionSet);
        }

        public IGraphQLQueryContentBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertyExpression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            ThrowIfInvalidGraphQLName(alias, "The alias is invalid.", nameof(alias));

            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            var fieldName = ConvertExpressionToFieldName(propertyExpression);

            return AddValidField(alias, fieldName, selectionSet);
        }

        public IGraphQLQueryContentBuilder AddField(string field)
        {
            ThrowIfInvalidGraphQLName(field, "The field name is invalid.", nameof(field));

            ThrowIfFieldNameIsNotAMemberOfTheClass(field);

            return AddValidField(field);
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field)
        {
            ThrowIfInvalidGraphQLName(alias, "The alias is invalid.", nameof(alias));
            ThrowIfInvalidGraphQLName(field, "The field name is invalid.", nameof(field));

            ThrowIfFieldNameIsNotAMemberOfTheClass(field);

            return AddValidField(alias, field);
        }

        public IGraphQLQueryContentBuilder AddField(string field, ISelectionSet selectionSet)
        {
            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            ThrowIfInvalidGraphQLName(field, "The field name is invalid.", nameof(field));

            ThrowIfFieldNameIsNotAMemberOfTheClass(field);

            return AddValidField(field, selectionSet);
        }

        public IGraphQLQueryContentBuilder AddField(string alias, string field, ISelectionSet selectionSet)
        {
            if (selectionSet == null)
            {
                throw new ArgumentNullException(nameof(selectionSet));
            }

            ThrowIfInvalidGraphQLName(alias, "The alias is invalid.", nameof(alias));
            ThrowIfInvalidGraphQLName(field, "The field name is invalid.", nameof(field));

            ThrowIfFieldNameIsNotAMemberOfTheClass(field);

            return AddValidField(alias, field, selectionSet);
        }

        public string Build()
        {
            var content = new StringBuilder();

            content.AppendLine("{");

            var selectionSetContent = BuildSelectionSetContent();

            if (!string.IsNullOrWhiteSpace(selectionSetContent))
            {
                content.AppendLine(selectionSetContent);
            }

            content.Append("}");

            return content.ToString();
        }

        private static void ThrowIfInvalidGraphQLName(string value, string message, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value) || !GraphQLNameRegex.IsMatch(value))
            {
                throw new ArgumentException(message, parameterName);
            }
        }

        private IGraphQLQueryContentBuilder<T> AddValidField(string fieldName)
        {
            _selections.Add(SelectionSet.Create(fieldName));

            return this;
        }

        private IGraphQLQueryContentBuilder<T> AddValidField(string alias, string fieldName)
        {
            _selections.Add(SelectionSet.Create(alias, fieldName));

            return this;
        }

        private IGraphQLQueryContentBuilder<T> AddValidField(string fieldName, ISelectionSet selectionSet)
        {
            _selections.Add(SelectionSet.Create(fieldName, selectionSet));

            return this;
        }

        private IGraphQLQueryContentBuilder<T> AddValidField(string alias, string fieldName, ISelectionSet selectionSet)
        {
            _selections.Add(SelectionSet.Create(alias, fieldName, selectionSet));

            return this;
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

        private void ThrowIfFieldNameIsNotAMemberOfTheClass(string fieldName)
        {
            if (_properties.All(property => property.Name != fieldName))
            {
                throw new ArgumentException(
                    $"Field name '{fieldName}' is not a property of type {typeof(T).FullName}.");
            }
        }

        private string BuildSelectionSetContent()
        {
            if (_selections.Count == 0)
            {
                return string.Empty;
            }

            var content = new StringBuilder();

            foreach (var selection in _selections)
            {
                if (content.Length != 0)
                {
                    content.AppendLine(",");
                }

                content.Append("  " + selection.Build());
            }

            return content.ToString();
        }
    }
}