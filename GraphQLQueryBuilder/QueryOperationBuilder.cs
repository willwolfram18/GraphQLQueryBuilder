using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GraphQLQueryBuilder.Guards;
using GraphQLQueryBuilder.Implementations.Language;

namespace GraphQLQueryBuilder
{
    public static class QueryOperationBuilder
    {
        public static IQueryOperationBuilder<T> ForSchema<T>(GraphQLOperationType type) where T : class
        {
            return new QueryOperationBuilder<T>(type);
        }

        public static IQueryOperationBuilder<T> ForSchema<T>(GraphQLOperationType type, string name) where T : class
        {
            return new QueryOperationBuilder<T>(type, name);
        }
    }

    internal class QueryOperationBuilder<T> : IQueryOperationBuilder<T>
        where T : class
    {
        private readonly ISelectionSetBuilder<T> _selectionSet = new SelectionSetBuilder<T>();

        public QueryOperationBuilder(GraphQLOperationType type) : this(type, null)
        {
        }

        public QueryOperationBuilder(GraphQLOperationType type, string name)
        {
            Type = type;
            Name = name.MustBeValidGraphQLName(nameof(name));
        }

        /// <inheritdoc />
        public GraphQLOperationType Type { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddScalarField<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            _selectionSet.AddScalarField(expression);

            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddScalarField<TProperty>(string alias, Expression<Func<T, TProperty>> expression)
        {
            _selectionSet.AddScalarField(alias, expression);

            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectField<TProperty>(Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            _selectionSet.AddObjectField(expression, selectionSet);

            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            _selectionSet.AddObjectField(alias, expression, selectionSet);

            return this;
        }

        public IQueryOperationBuilder<T> AddScalarCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            _selectionSet.AddScalarCollectionField(expression);

            return this;
        }

        public IQueryOperationBuilder<T> AddScalarCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            _selectionSet.AddScalarCollectionField(alias, expression);

            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            _selectionSet.AddObjectCollectionField(expression, selectionSet);

            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            _selectionSet.AddObjectCollectionField(alias, expression, selectionSet);

            return this;
        }

        /// <inheritdoc />
        public IGraphQLOperation<T> Build()
        {
            var selectionSet = _selectionSet.Build();

            return new GraphQLQueryOperation<T>(Type, Name, selectionSet);
        }
    }
}
