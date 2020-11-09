using System;
using System.Collections.Generic;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IArgumentCollection : ICollection<IArgument>, IReadOnlyDictionary<string, IArgumentValue>
    {
        /// <summary>Adds the elements of the specified collection to the end of the <see cref="IArgumentCollection" />.</summary>
        /// <param name="collection">
        /// The collection whose elements should be added to the end of the <see cref="IArgumentCollection" />.
        /// The collection itself cannot be null, and cannot contain elements that are null.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is null.</exception>
        void AddRange(IEnumerable<IArgument> collection);
    }
}