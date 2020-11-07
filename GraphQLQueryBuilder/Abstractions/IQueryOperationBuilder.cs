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

        IQueryOperationBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression);

        IQueryOperationBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> expression);

        IQueryOperationBuilder<T> AddField<TProperty>(Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        IQueryOperationBuilder<T> AddField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        IQueryOperationBuilder<T> AddCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        IQueryOperationBuilder<T> AddCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class;

        IGraphQLOperation<T> Build();
    }
}
