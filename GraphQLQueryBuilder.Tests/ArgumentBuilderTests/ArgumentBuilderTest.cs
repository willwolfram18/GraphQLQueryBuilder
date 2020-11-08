using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using GraphQLQueryBuilder.Abstractions.Language;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.ArgumentBuilderTests
{
    public abstract class ArgumentBuilderTest<T, TArgumentValue>
        where TArgumentValue : IArgumentValue
    {
        protected abstract T ArgumentValue { get; set; }

        protected abstract IArgument BuildArgument(string name, T value);

        protected abstract void ValidateArgumentValue(AndWhichConstraint<ObjectAssertions, TArgumentValue> argumentValueAssertions);

        [Test]
        public void Then_Argument_Name_Cannot_Be_Null_Or_White_Space(
            [Values(null, "", "   ", "  \n \t")] string argumentName)
        {
            Action method = () => BuildArgument(argumentName, ArgumentValue);

            using (new AssertionScope())
            {
                var exceptionAssertion = FluentActions.Invoking(method).Should().ThrowExactly<ArgumentException>()
                    .WithMessage("A GraphQL argument name cannot be null or white space.*");
                
                exceptionAssertion.And.ParamName.Should().Be("name");
                exceptionAssertion.And.HelpLink.Should().Be("https://spec.graphql.org/June2018/#Name");
            }
            
        }

        [InvalidGraphQLNamesTestCaseSource]
        public void Then_Argument_Name_Must_Be_A_Valid_GraphQL_Name(string name, string because)
        {
            Action method = () => BuildArgument(name, ArgumentValue);

            FluentActions.Invoking(method).Should().ThrowInvalidGraphQLNameException("name", because);
        }
        
        [Test]
        public void Then_Argument_Value_Should_Match_Provided_String()
        {
            var argument = BuildArgument("myName", ArgumentValue);

            using (new AssertionScope())
            {
                argument.Should().NotBeNull();
                argument?.Name.Should().Be("myName");
                
                var argumentValue = argument?.Value.Should().BeAssignableTo<TArgumentValue>();

                ValidateArgumentValue(argumentValue);
            }
        }
    }
}