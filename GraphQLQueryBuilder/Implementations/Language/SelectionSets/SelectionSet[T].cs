using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class SelectionSet<T> : ISelectionSet<T> where T : class
    {
        public SelectionSet(IEnumerable<ISelectionSetItem> selections)
        {
            Selections = selections ?? throw new ArgumentNullException(nameof(selections));
            
            if (!selections.Any())
            {
                throw new InvalidOperationException("A selection set must include one or more fields.")
                {
                    HelpLink = "https://spec.graphql.org/June2018/#SelectionSet"
                };
            }
        }

        /// <inheritdoc />
        public IEnumerable<ISelectionSetItem> Selections { get; }
    }
}
