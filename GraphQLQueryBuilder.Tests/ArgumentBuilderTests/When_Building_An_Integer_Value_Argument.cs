using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using GraphQLQueryBuilder.Abstractions.Language;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.ArgumentBuilderTests
{
    public class When_Building_An_Integer_Value_Argument : ArgumentBuilderTest<int?, IIntegerArgumentValue>
    {
        protected override int? ArgumentValue { get; set; }

        protected override IArgument BuildArgument(string name, int? value) =>
            ArgumentBuilder.Build(name, value);

        protected override void ValidateArgumentValue(AndWhichConstraint<ObjectAssertions, IIntegerArgumentValue> argumentValueAssertions)
        {
            argumentValueAssertions.Which.Value.Should().Be(ArgumentValue.Value);
        }

        [SetUp]
        public void SetUp()
        {
            ArgumentValue = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        [Test]
        public void If_Integer_Value_Is_Null_Then_Argument_Value_Is_A_NullArgumentValue()
        {
            var argument = BuildArgument("myName", null);

            using (new AssertionScope())
            {
                argument.Should().NotBeNull();
                argument?.Name.Should().Be("myName");
                argument?.Value.Should().BeAssignableTo<INullArgumentValue>("because the value was null");
            }
        }
    }
}