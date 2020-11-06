using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using GraphQLQueryBuilder.Abstractions.Language;

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

        public ISelectionSetBuilder<T> AddField<TProperty>(Expression<System.Func<T, TProperty>> propertySelection)
        {
            return AddField(null, propertySelection);
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<System.Func<T, TProperty>> propertySelection)
        {
            if (propertySelection == null)
            {
                throw new ArgumentNullException(nameof(propertySelection));
            }

            if (!string.IsNullOrWhiteSpace(alias) && !ValidAliasName.IsMatch(alias))
            {
                throw new ArgumentException("Provided alias does not comply with GraphQL's Name specification.")
                {
                    HelpLink = "https://spec.graphql.org/June2018/#Name"
                };
            }
            //throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(Expression<System.Func<T, TProperty>> propertySelection, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<System.Func<T, TProperty>> propertySelection, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(Expression<System.Func<T, System.Collections.Generic.IEnumerable<TProperty>>> propertySelection, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<System.Func<T, System.Collections.Generic.IEnumerable<TProperty>>> propertySelection, ISelectionSet<TProperty> selectionSet) where TProperty : class
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