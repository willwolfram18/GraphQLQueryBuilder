using System;

namespace GraphQLQueryBuilder.Abstractions.Language
{
    public enum GraphQLType
    {
        [IsScalar]
        Integer,
        [IsScalar]
        Float,
        [IsScalar]
        String,
        [IsScalar]
        Boolean,
        [IsScalar]
        Enum,
        [IsScalar]
        Uuid,
        Object,
        InputObject
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal class IsScalarAttribute : Attribute
    {
    }
}