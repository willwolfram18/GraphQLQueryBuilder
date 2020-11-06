using System;
using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests
{
    public class When_Adding_A_Field : SelectionSetBuilderTest
    {
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            ISelectionSetBuilder<Customer> builder = null;

            Action method = () => builder.AddField<Guid>(null);

            Invoking(method).Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void If_Property_Type_Is_A_Class_Then_InvalidOperationException_Is_Thrown_Stating_The_Overload_Should_Be_Used()
        {
            ISelectionSetBuilder<Customer> builder = null;

            Action method = () => builder.AddField(customer => customer.CustomerContact);

            Invoking(method).Should().ThrowExactly<InvalidOperationException>("because there is an overload for class properties")
                .WithMessage("When selecting a property that is a class, please use the AddField method that takes an ISelectionSet.");
        }

        [TestCaseSource(typeof(InvalidGraphQLNames), nameof(InvalidGraphQLNames))]
        public void If_Property_Alias_Has_An_Invalid_Name_Then_ArgumentException_Is_Thrown_Stating_Alias_Name_Is_Not_Valid(
            string alias, string because)
        {
            ISelectionSetBuilder<Customer> builder = null;

            Action method = () => builder.AddField(alias, customer => customer.Id);

            Invoking(method).Should().ThrowExactly<ArgumentException>(because);
        }

        [Test]
        public void Then_Added_Properties_Are_In_The_Selection_Set()
        {
            ISelectionSetBuilder<Customer> builder = null;

            builder.AddField(customer => customer.Id)
                .AddField("acctNum", customer => customer.AccountNumber);

            var selectionSet = builder.Build();

            var expectedSelections = new IFieldSelectionItem[]
            {
                Mock.Of<IFieldSelectionItem>(item => item.FieldName == nameof(Customer.Id)),
                Mock.Of<IFieldSelectionItem>(item => item.Alias == "acctNum" && item.FieldName == nameof(Customer.AccountNumber))
            };

            selectionSet.Selections.Should().BeEquivalentTo(expectedSelections);

            IQueryRenderer renderer = null;
            var result = renderer.Render(selectionSet);

            Assert.Fail("Compare with snapshooter");
        }
    }
}