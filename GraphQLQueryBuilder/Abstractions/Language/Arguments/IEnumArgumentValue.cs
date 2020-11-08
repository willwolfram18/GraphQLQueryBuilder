using System;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IEnumArgumentValue<T> : IArgumentValue where T : Enum
    {
        T Value { get; }
    }
}