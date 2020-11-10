using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using GraphQLQueryBuilder.Abstractions.Language;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.ArgumentBuilderTests
{
    public class When_Building_A_String_Value_Argument : ArgumentBuilderTest<string, IStringArgumentValue>
    {
        protected override string ArgumentValue { get; set; }

        protected override IArgument BuildArgument(string name, string value) =>
            ArgumentBuilder.Build(name, value);

        protected override void ValidateArgumentValue(AndWhichConstraint<ObjectAssertions, IStringArgumentValue> argumentValueAssertions)
        {
            argumentValueAssertions.Which.Value.Should().Be(ArgumentValue);
        }


        [SetUp]
        public void SetUp()
        {
            ArgumentValue = $"The value is {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        }

        [Test]
        public void If_String_Value_Is_Null_Then_Argument_Value_Is_A_NullArgumentValue()
        {
            var argument = BuildArgument("myName", null);

            using (new AssertionScope())
            {
                argument.Should().NotBeNull();
                argument?.Name.Should().Be("myName");
                argument?.Value.Should().BeAssignableTo<INullArgumentValue>("because the value is null");
            }
        }
    }
}