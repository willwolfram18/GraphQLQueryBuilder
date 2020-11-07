using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;

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
    }
}