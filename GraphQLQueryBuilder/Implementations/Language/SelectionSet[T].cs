using GraphQLQueryBuilder.Abstractions.Language;
using System;
using System.Collections.Generic;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal class SelectionSet<T> : ISelectionSet<T> where T : class
    {
        public SelectionSet(IEnumerable<ISelectionSetItem> selections)
        {
            Selections = selections ?? throw new ArgumentNullException(nameof(selections));
        }

        /// <inheritdoc />
        public IEnumerable<ISelectionSetItem> Selections { get; }
    }
}
