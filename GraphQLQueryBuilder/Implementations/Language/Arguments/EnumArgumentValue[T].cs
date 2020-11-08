using System;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations.Language
{
    public class EnumArgumentValue<T> : ArgumentValue<T>, IEnumArgumentValue<T>
        where T : Enum
    {
        public EnumArgumentValue(T value) : base(value)
        {
        }

        public object Value => base.Value;
    }
}