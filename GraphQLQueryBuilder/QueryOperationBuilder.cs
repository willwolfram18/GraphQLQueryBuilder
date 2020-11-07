﻿using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;
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
            Name = name;
        }

        /// <inheritdoc />
        public GraphQLOperationType Type { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddScalarField<TProperty>(Expression<Func<T, TProperty>> expression)
        {
            throw new NotImplementedException();
//            _selectionSet.AddField(expression);
//
//            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddScalarField<TProperty>(string alias, Expression<Func<T, TProperty>> expression)
        {
            throw new NotImplementedException();
//            _selectionSet.AddField(expression);
//
//            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectField<TProperty>(Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new NotImplementedException();
//            _selectionSet.AddField(expression);
//
//            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectField<TProperty>(string alias, Expression<Func<T, TProperty>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new NotImplementedException();
//            _selectionSet.AddField(expression);
//
//            return this;
        }

        public IQueryOperationBuilder<T> AddScalarCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            throw new NotImplementedException();
        }

        public IQueryOperationBuilder<T> AddScalarCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectCollectionField<TProperty>(Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new NotImplementedException();
//            _selectionSet.AddField(expression);
//
//            return this;
        }

        /// <inheritdoc />
        public IQueryOperationBuilder<T> AddObjectCollectionField<TProperty>(string alias, Expression<Func<T, IEnumerable<TProperty>>> expression, ISelectionSet<TProperty> selectionSet) where TProperty : class
        {
            throw new NotImplementedException();
//            _selectionSet.AddField(expression);
//
//            return this;
        }

        /// <inheritdoc />
        public IGraphQLOperation<T> Build()
        {
            throw new NotImplementedException();
        }
    }
}
