using System;
using FluentAssertions;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.QueryOperationBuilderTests
{
    public class When_Creating_An_Operation_Builder : QueryOperationBuilderTest
    {
        public When_Creating_An_Operation_Builder(GraphQLOperationType operationType) : base(operationType)
        {
        }
        
        [Test]
        public void Then_Operation_Type_Matches_Parameter()
        {
            var query = CreateBuilderFor<SimpleSchema>()
                .AddField(schema => schema.Version)
                .Build();

            query.Should().NotBeNull();
            query.Type.Should().Be(OperationTypeForFixture);
            query.Name.Should().BeNull();
        }

        [Test]
        public void Then_Operation_Type_And_Name_Match_Parameters(
            [Values(null, "", "    ", "  \n \t", "MyOperation")] string operationName)
        {
            var query = CreateBuilderFor<SimpleSchema>(operationName)
                .AddField(schema => schema.Version)
                .Build();

            var expectedOperationName = string.IsNullOrWhiteSpace(operationName) ? null : operationName;
            
            query.Should().NotBeNull();
            query.Type.Should().Be(OperationTypeForFixture);
            query.Name.Should().Be(expectedOperationName);
        }

        [InvalidOperationNamesTestCaseSource]
        public void
            If_Operation_Name_Is_Not_A_Valid_GraphQL_Name_Then_ArgumentException_Is_Thrown_Stating_Operation_Name_Is_Not_Valid(
                string operationName, string because)
        {
            Action method = () => CreateBuilderFor<SimpleSchema>(operationName);
            
            Invoking(method).Should().ThrowExactly<ArgumentException>(because)
                .Where(e => e.ParamName == "alias", "because the alias parameter is the problem")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#Name", "because this is the link to the GraphQL 'Name' spec");
        }
    }
}