using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

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
        private static readonly Regex ValidAliasName = new Regex(@"^[_A-Za-z][_0-9A-Za-z]*$");

        private readonly PropertyInfo[] _properties = typeof(T).GetProperties();

        public ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            return AddField(null, expression);
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> expression)
        {
            if (!string.IsNullOrWhiteSpace(alias) && !ValidAliasName.IsMatch(alias))
            {
                throw new ArgumentException("Provided alias does not comply with GraphQL's Name specification.")
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

            if (_properties.All(property => property.Name != memberExpression.Member.Name))
            {
                throw new InvalidOperationException($"Property '{memberExpression.Member.Name}' is not a member of type '{typeof(T).FullName}'.");
            }
            //throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, System.Collections.Generic.IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, System.Collections.Generic.IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSet<T> Build()
        {
            //throw new System.NotImplementedException();
            return null;
        }
    }
}