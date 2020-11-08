using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using GraphQLQueryBuilder.Abstractions.Language;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.ArgumentBuilderTests
{
    public class When_Building_A_Float_Value_Argument_From_A_Double : ArgumentBuilderTest<double?, IFloatArgumentValue>
    {
        private static readonly Random _numberGenerator = new Random();
        protected override double? ArgumentValue { get; set; }

        protected override IArgument BuildArgument(string name, double? value) => ArgumentBuilder.Build(name, value);

        protected override void ValidateArgumentValue(AndWhichConstraint<ObjectAssertions, IFloatArgumentValue> argumentValueAssertions)
        {
            argumentValueAssertions.Which.Value.Should().Be(ArgumentValue.Value);
        }

        [SetUp]
        public void SetUp()
        {
            ArgumentValue = _numberGenerator.NextDouble();
        }
        
        [Test]
        public void If_Double_Value_Is_Null_Then_Argument_Value_Is_A_NullArgumentValue()
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