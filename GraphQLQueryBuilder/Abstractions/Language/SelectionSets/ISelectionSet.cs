using System.Collections.Generic;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface ISelectionSet
    {
        IEnumerable<ISelectionSetItem> Selections { get; }
    }
}