using System;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IEnumArgumentValue<T> : IArgumentValue, ILiteralArgumentValue where T : Enum
    {
        T Value { get; }
    }
}