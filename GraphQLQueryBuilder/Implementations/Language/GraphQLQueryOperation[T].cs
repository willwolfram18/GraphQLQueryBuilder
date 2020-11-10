using System;
using System.Linq;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class GraphQLQueryOperation<T> : IGraphQLOperation<T> where T : class
    {
        public GraphQLQueryOperation(GraphQLOperationType type, string name, ISelectionSet<T> selectionSet)
        {
            Type = type;
            Name = name;
            SelectionSet = selectionSet ?? throw new ArgumentNullException(nameof(selectionSet));

            if (!selectionSet.Selections.Any())
            {
                // TODO throw? 
            }
        }
        
        public GraphQLOperationType Type { get; }
        
        public string Name { get; }
        
        ISelectionSet IGraphQLOperation.SelectionSet => SelectionSet;

        public ISelectionSet<T> SelectionSet { get; }
    }
}