using System;
using System.Collections.Generic;
using System.Linq;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder.Implementations
{
    public static class TypeExtensions
    {
        private static IReadOnlyCollection<Type> _integerTypes;
        private static IReadOnlyCollection<Type> IntegerTypes
        {
            get
            {
                return _integerTypes ??= new []
                {
                    typeof(short),
                    typeof(Int16),
                    typeof(ushort),
                    typeof(UInt16),
                    typeof(int),
                    typeof(Int32),
                    typeof(uint),
                    typeof(UInt32),
                    typeof(long),
                    typeof(Int64),
                    typeof(ulong),
                    typeof(UInt64)
                };
            }
        }
        
        private static IReadOnlyCollection<Type> _floatTypes;
        private static IReadOnlyCollection<Type> FloatTypes
        {
            get
            {
                return _floatTypes ??= new []
                {
                    typeof(float),
                    typeof(decimal),
                    typeof(Decimal),
                    typeof(double),
                    typeof(Double)
                };
            }
        }
        
        private static IReadOnlyCollection<Type> _booleanTypes;
        private static IReadOnlyCollection<Type> BooleanTypes
        {
            get
            {
                return _booleanTypes ??= new []
                {
                    typeof(bool),
                    typeof(Boolean)
                };
            }
        }
        
        private static IReadOnlyCollection<Type> _stringTypes;
        private static IReadOnlyCollection<Type> StringTypes
        {
            get
            {
                return _stringTypes ??= new []
                {
                    typeof(string),
                    typeof(String),
                    typeof(char[]),
                    typeof(char*),
                    typeof(Span<char>)
                };
            }
        }
        
        public static GraphQLType ToGraphQLType(this Type type)
        {
            if (type.IsEnum)
            {
                return GraphQLType.Enum;
            }
            
            if (IntegerTypes.Contains(type))
            {
                return GraphQLType.Integer;
            }

            if (FloatTypes.Contains(type))
            {
                return GraphQLType.Float;
            }

            if (StringTypes.Contains(type))
            {
                return GraphQLType.String;
            }

            if (BooleanTypes.Contains(type))
            {
                return GraphQLType.Boolean;
            }
            
            if ((type.IsClass || type.IsInterface) && type != typeof(string))
            {
                return GraphQLType.InputObject;
            }
            
            throw new NotImplementedException($"Unknown CLR type '{type.FullName}'.");
        }
    }
}