using System;
using FluentAssertions;
using FluentAssertions.Execution;
using GraphQLQueryBuilder.Abstractions.Language;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.ArgumentBuilderTests
{
    public class When_Building_A_String_Value_Argument
    {
        private string _argumentValue;

        [SetUp]
        public void SetUp()
        {
            _argumentValue = $"The value is {DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        }
        
        [Test]
        public void Then_Argument_Name_Cannot_Be_Null_Or_White_Space(
            [Values(null, "", "   ", "  \n \t")] string argumentName)
        {
            Action method = () => ArgumentBuilder.Build(argumentName, _argumentValue);

            using (new AssertionScope())
            {
                var exceptionAssertion = Invoking(method).Should().ThrowExactly<ArgumentException>()
                    .WithMessage("A GraphQL argument name cannot be null or white space.*");
                
                exceptionAssertion.And.ParamName.Should().Be("name");
                exceptionAssertion.And.HelpLink.Should().Be("http://spec.graphql.org/June2018/#Name");
            }
            
        }

        [InvalidGraphQLNamesTestCaseSource]
        public void Then_Argument_Name_Must_Be_A_Valid_GraphQL_Name(string name, string because)
        {
            Action method = () => ArgumentBuilder.Build(name, _argumentValue);

            Invoking(method).Should().ThrowInvalidGraphQLNameException("name", because);
        }

        [Test]
        public void If_String_Value_Is_Null_Then_Argument_Value_Is_A_NullArgumentValue()
        {
            var argument = ArgumentBuilder.Build("myName", null);

            using (new AssertionScope())
            {
                argument.Should().NotBeNull();
                argument?.Name.Should().Be("myName");
                argument?.Value.Should().BeOfType<INullArgumentValue>();
            }
        }

        [Test]
        public void Then_Argument_Value_Should_Match_Provided_String()
        {
            var argument = ArgumentBuilder.Build("myName", _argumentValue);

            using (new AssertionScope())
            {
                argument.Should().NotBeNull();
                argument?.Name.Should().Be("myName");
                argument?.Value.Should().BeOfType<IStringArgumentValue>()
                    .Which.Value.Should().Be(_argumentValue);
            }
        }
    }
}