using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder
{
    public interface ISelectionSetBuilder<T> where T : class
    {
        ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertySelection);

        ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertySelection);

        ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> propertySelection, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> propertySelection, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> propertySelection, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> propertySelection, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSet<T> Build();
    }
}