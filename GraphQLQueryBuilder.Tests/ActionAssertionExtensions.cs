using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Specialized;

namespace GraphQLQueryBuilder.Tests
{
    public static class ActionAssertionExtensions
    {
        public static ExceptionAssertions<ArgumentException> ThrowInvalidGraphQLNameException(this ActionAssertions actionShould, string argumentName, string because)
        {
            using (new AssertionScope())
            {
                var exceptionAssertions = actionShould.ThrowExactly<ArgumentException>(because)
                    .WithMessage("Provided name does not comply with GraphQL's name specification.*");
                
                exceptionAssertions.And.ParamName.Should().Be(argumentName,"because this is the argument that violates GraphQL's name specification");
                exceptionAssertions.And.HelpLink.Should().Be("https://spec.graphql.org/June2018/#Name",
                    "because this is the GraphQL Name specification");

                return exceptionAssertions;
            }
        }
    }
}