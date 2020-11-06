using System.Collections.Generic;

namespace GraphQLQueryBuilder
{
    public interface ISelectionSet
    {
        IEnumerable<IFieldSelectionItem> Selections { get; }
    }
}