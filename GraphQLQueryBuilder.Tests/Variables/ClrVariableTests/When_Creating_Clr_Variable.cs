using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Implementations.Language;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.Variables.ClrVariableTests
{
    public class When_Creating_Clr_Variable
    {
        public static IEnumerable<Type> KnownClrTypes
        {
            get
            {
                return IntegerTypes.Concat(FloatTypes)
                    .Concat(StringTypes)
                    .Concat(BooleanTypes)
                    .Concat(EnumObjectTypes)
                    .Concat(InputObjectTypes);
            }
        }
        
        [Test]
        public void Then_ClrType_And_Name_Match_Constructor_Arguments(
            [ValueSource(nameof(KnownClrTypes))] Type clrType)
        {
            const string expectedName = "myVariableName";

            var variable = new ClrVariable(expectedName, clrType);

            variable.Name.Should().Be(expectedName);
            variable.ClrType.Should().Be(clrType);
        }
        
        public static IEnumerable<Type> IntegerTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(short),
                    typeof(Int16),
                    typeof(int),
                    typeof(Int32),
                    typeof(long),
                    typeof(Int64),
                    typeof(ushort),
                    typeof(UInt16),
                    typeof(uint),
                    typeof(UInt32),
                    typeof(ulong),
                    typeof(UInt64)
                };
            }
        }
        
        [Test]
        public void If_Clr_Type_Is_An_Integer_Then_GraphQL_Type_Is_An_Integer(
            [ValueSource(nameof(IntegerTypes))] Type clrType)
        {
            ClrTypeShouldBe(clrType, GraphQLType.Integer);
        }

        public static IEnumerable<Type> FloatTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(float),
                    typeof(double),
                    typeof(Double)
                };
            }
        }
        
        [Test]
        public void If_Clr_Type_Is_An_Decimal_Type_Then_GraphQL_Type_Is_A_Float(
            [ValueSource(nameof(FloatTypes))] Type clrType)
        {
            ClrTypeShouldBe(clrType, GraphQLType.Float);
        }

        public static IEnumerable<Type> StringTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(string),
                    typeof(String),
                    typeof(char[]),
                    typeof(char*),
                    typeof(Span<char>)
                };
            }
        }

        [Test]
        public void If_Clr_Type_Is_A_Collection_Of_Characters_Then_GraphQL_Type_Is_A_String(
            [ValueSource(nameof(StringTypes))] Type clrType)
        {
            ClrTypeShouldBe(clrType, GraphQLType.String);
        }

        public static IEnumerable<Type> BooleanTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(bool),
                    typeof(Boolean)
                };
            }
        }

        [Test]
        public void If_Clr_Type_Is_A_Boolean_Then_GraphQL_Type_Is_A_String(
            [ValueSource(nameof(BooleanTypes))] Type clrType)
        {
            ClrTypeShouldBe(clrType, GraphQLType.Boolean);
        }

        public static IEnumerable<Type> EnumObjectTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(CustomerStatus),
                    typeof(NestedEnum)
                };
            }
        }

        [Test]
        public void If_Clr_Type_Is_An_Enum_Then_GraphQL_Type_Is_An_Enum(
            [ValueSource(nameof(EnumObjectTypes))] Type clrType)
        {
            ClrTypeShouldBe(clrType, GraphQLType.Enum);
        }

        public static IEnumerable<Type> InputObjectTypes
        {
            get
            {
                return new Type[]
                {
                    typeof(Address),
                    typeof(PhoneNumber),
                    typeof(IFoobar)
                };
            }
        }

        [Test]
        public void If_Clr_Type_Is_A_Class_Or_Interface_Then_GraphQL_Type_Is_An_InputObject(
            [ValueSource(nameof(InputObjectTypes))] Type clrType)
        {
            ClrTypeShouldBe(clrType, GraphQLType.InputObject);
        }

        private static void ClrTypeShouldBe(Type clrType, GraphQLType expectedType)
        {
            var variable = new ClrVariable("foo", clrType);

            variable.Type.Should().Be(expectedType);
        }

        public enum NestedEnum
        {
            Foo,
            Bar
        }

        public interface IFoobar
        {
        }
    }
}