using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GraphQLQueryBuilder.Abstractions
{
    public interface IQueryOperationBuilder<T> where T : class
    {
        GraphQLOperationType Type { get; }

        string Name { get; }

        IQueryOperationBuilder<T> AddScalarField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, IArgumentCollection arguments);

        IQueryOperationBuilder<T> AddObjectField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, IArgumentCollection arguments, ISelectionSet<TProperty> selectionSet) where TProperty : class;
        
        IQueryOperationBuilder<T> AddScalarCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, IArgumentCollection arguments);

        IQueryOperationBuilder<T> AddObjectCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        IQueryOperationBuilder<T> AddObjectCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        IGraphQLOperation<T> Build();
    }
}
