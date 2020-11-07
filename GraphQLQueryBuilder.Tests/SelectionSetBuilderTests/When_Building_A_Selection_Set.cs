using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Building_A_Selection_Set
    {
        [Test]
        public void Then_Added_Properties_Are_In_The_Selection_Set()
        {
            var selectionSet = SelectionSetBuilder.Of<Customer>()
                .AddField(customer => customer.Id)
                .AddField("acctNum", customer => customer.AccountNumber)
                .Build();

            var expectedSelections = new List<IFieldSelectionItem>
            {
                Mock.Of<IFieldSelectionItem>(item => item.FieldName == nameof(Customer.Id)),
                Mock.Of<IFieldSelectionItem>(item => item.Alias == "acctNum" && item.FieldName == nameof(Customer.AccountNumber))
            };

            selectionSet.Should().NotBeNull();
            selectionSet.Selections.Where(selection => selection is IFieldSelectionItem)
                .Cast<IFieldSelectionItem>()
                .Should().BeEquivalentTo(expectedSelections);
        }
        
        [Test]
        public void If_No_Fields_Are_Selected_Before_Building_Then_An_InvalidOperationException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Invoking(builder.Build).Should()
                .ThrowExactly<InvalidOperationException>("because a selection set must include 1 or more fields")
                .WithMessage("A selection set must include one or more fields.")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#SelectionSet");
        }
    }
}