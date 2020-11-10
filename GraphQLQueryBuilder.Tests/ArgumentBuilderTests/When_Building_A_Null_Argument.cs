using System;
using FluentAssertions;
using FluentAssertions.Execution;
using GraphQLQueryBuilder.Abstractions.Language;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.ArgumentBuilderTests
{
    public class When_Building_A_Null_Argument
    {
        [Test]
        public void Then_Argument_Name_Cannot_Be_Null_Or_White_Space(
            [Values(null, "", "   ", "  \n \t")] string argumentName)
        {
            Action method = () => ArgumentBuilder.Build(argumentName);

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
            Action method = () => ArgumentBuilder.Build(name);

            FluentActions.Invoking(method).Should().ThrowInvalidGraphQLNameException("name", because);
        }

        [Test]
        public void Then_Argument_Value_Is_A_NullArgumentValue()
        {
            var argument = ArgumentBuilder.Build("myName");

            using (new AssertionScope())
            {
                argument.Should().NotBeNull();
                argument?.Name.Should().Be("myName");
                argument?.Value.Should().BeAssignableTo<INullArgumentValue>();
            }
        }
    }
}