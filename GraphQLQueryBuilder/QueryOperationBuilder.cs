﻿using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Guards;
using GraphQLQueryBuilder.Implementations.Language;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
        public IQueryOperationBuilder<T> AddScalarField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, IArgumentCollection arguments)
        {
            _selectionSet.AddScalarField(alias, expression, arguments);

            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, IArgumentCollection arguments,
            ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            _selectionSet.AddObjectField(alias, expression, selectionSet);

            return this;
        }

        public IQueryOperationBuilder<T> AddScalarCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression,
            IArgumentCollection arguments)
        {
            _selectionSet.AddScalarCollectionField(alias, expression, arguments);

            return this;
        }

        public IQueryOperationBuilder<T> AddObjectCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression,
            IArgumentCollection arguments, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            _selectionSet.AddObjectCollectionField(alias, expression, arguments, selectionSet);

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
