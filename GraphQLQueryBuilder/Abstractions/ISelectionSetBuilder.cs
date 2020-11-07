using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder.Abstractions
{
    public interface ISelectionSetBuilder<T> where T : class
    {
        ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression);

        ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> expression);

        ISelectionSetBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSetBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSetBuilder<T> AddCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression);
        
        ISelectionSetBuilder<T> AddCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression);
        
        ISelectionSetBuilder<T> AddCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSetBuilder<T> AddCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSet<T> Build();
    }
}