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
        public ISelectionSetBuilder<T> AddField<TProperty>(System.Linq.Expressions.Expression<System.Func<T, TProperty>> propertySelection)
        {
            //throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, System.Linq.Expressions.Expression<System.Func<T, TProperty>> propertySelection)
        {
            //throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(System.Linq.Expressions.Expression<System.Func<T, TProperty>> propertySelection, Abstractions.Language.ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, System.Linq.Expressions.Expression<System.Func<T, TProperty>> propertySelection, Abstractions.Language.ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<TProperty>>> propertySelection, Abstractions.Language.ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new System.NotImplementedException();
            return this;
        }

        public ISelectionSetBuilder<T> AddField<TProperty>(string alias, System.Linq.Expressions.Expression<System.Func<T, System.Collections.Generic.IEnumerable<TProperty>>> propertySelection, Abstractions.Language.ISelectionSet<TProperty> selectionSet) where TProperty : class
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