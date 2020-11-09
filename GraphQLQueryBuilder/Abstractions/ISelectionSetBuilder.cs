using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder.Abstractions
{
    public interface ISelectionSetBuilder<T> where T : class
    {
        ISelectionSetBuilder<T> AddScalarField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, IArgumentCollection arguments);

        ISelectionSetBuilder<T> AddObjectField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, IArgumentCollection arguments, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSetBuilder<T> AddScalarCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, IArgumentCollection arguments);
        
        ISelectionSetBuilder<T> AddObjectCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, IArgumentCollection arguments, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        ISelectionSet<T> Build();
    }
}