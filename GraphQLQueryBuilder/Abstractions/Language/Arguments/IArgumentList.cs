using System.Collections.Generic;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IArgumentList : ICollection<IArgument>
    {
        /// <inheritdoc cref="List{T}.AddRange"/>
        void AddRange(IEnumerable<IArgument> collection);
    }
}