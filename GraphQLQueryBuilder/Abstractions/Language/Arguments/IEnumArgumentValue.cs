using System;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public interface IEnumArgumentValue : IArgumentValue, ILiteralArgumentValue
    {
        object Value { get; }
    }
    
    public interface IEnumArgumentValue<out T> : IEnumArgumentValue where T : Enum
    {
        new T Value { get; }
    }
}