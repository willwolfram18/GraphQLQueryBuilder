using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using GraphQLQueryBuilder.Abstractions.Language;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.ArgumentBuilderTests
{
    [TestFixtureSource(nameof(BooleanValues))]
    public class When_Building_A_Boolean_Value_Argument : ArgumentBuilderTest<bool?, IBooleanArgumentValue>
    {
        public When_Building_A_Boolean_Value_Argument(bool testFixtureValue)
        {
            ArgumentValue = testFixtureValue;
        }
        
        public static IEnumerable<bool> BooleanValues => new[] {true, false};
        
        protected override bool? ArgumentValue { get; set; }

        protected override IArgument BuildArgument(string name, bool? value) =>
            ArgumentBuilder.Build(name, value);

        protected override void ValidateArgumentValue(AndWhichConstraint<ObjectAssertions, IBooleanArgumentValue> argumentValueAssertions)
        {
            argumentValueAssertions.Which.Value.Should().Be(ArgumentValue.Value);
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