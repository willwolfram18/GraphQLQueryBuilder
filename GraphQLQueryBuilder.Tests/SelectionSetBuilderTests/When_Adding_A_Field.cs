using FluentAssertions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Adding_A_Field : SelectionSetBuilderTest
    {
        [Test]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown()
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Action method = () => builder.AddField<Guid>(null);

            Invoking(method).Should().ThrowExactly<ArgumentNullException>();
        }

        public static IEnumerable<AddFieldToSelectionSetTestCase<Customer>> PropertyTypeIsAClass
        {
            get
            {
                yield return new AddFieldToSelectionSetTestCase<Customer>(
                    builder => builder.AddField(customer => customer.CustomerContact),
                    "Adding a field that is a class."
                );
                yield return new AddFieldToSelectionSetTestCase<Customer>(
                    builder => builder.AddField("foo", customer => customer.CustomerContact),
                    "Adding a field using an alias that is a class."
                );
            }
        }

        [TestCaseSource(nameof(PropertyTypeIsAClass))]
        public void If_Property_Type_Is_A_Class_Then_InvalidOperationException_Is_Thrown_Stating_The_Overload_Should_Be_Used(
            AddFieldToSelectionSetTestCase<Customer> testData)
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Invoking(() => testData.FieldAddition(builder)).Should().ThrowExactly<InvalidOperationException>("because there is an overload for class properties")
                .WithMessage("When selecting a property that is a class, please use the AddField method that takes an ISelectionSet.");
        }

        public static IEnumerable<AddFieldToSelectionSetTestCase<Customer>> FieldIsNotAMemberOfTheType
        {
            get
            {
                yield return new AddFieldToSelectionSetTestCase<Customer>(
                    builder => builder.AddField(_ => "hello"),
                    "Adding a field that isn't on the model."
                );
                yield return new AddFieldToSelectionSetTestCase<Customer>(
                    builder => builder.AddField("foo", _ => "hello"),
                    "Adding a field using an alias that isn't on the model."
                );
            }
        }

        [Test]
        public void If_Expression_Evaluation_Is_Not_A_Property_On_The_Class_Then_An_InvalidOperationException_Is_Thrown(
            AddFieldToSelectionSetTestCase<Customer> testData)
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Invoking(() => testData.FieldAddition(builder)).Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"Property '*' is not a member of type '{typeof(Customer).FullName}'.");
        }

        [InvalidAliasNamesTestCaseSource]
        public void If_Property_Alias_Has_An_Invalid_Name_Then_ArgumentException_Is_Thrown_Stating_Alias_Name_Is_Not_Valid(
            string alias, string because)
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Action method = () => builder.AddField(alias, customer => customer.Id);

            Invoking(method).Should().ThrowExactly<ArgumentException>(because);
        }

        [Test]
        public void Then_Added_Properties_Are_In_The_Selection_Set()
        {
            var selectionSet = SelectionSetBuilder.Of<Customer>()
                .AddField(customer => customer.Id)
                .AddField("acctNum", customer => customer.AccountNumber)
                .Build();

            var expectedSelections = new List<ISelectionSetItem>
            {
                Mock.Of<IFieldSelectionItem>(item => item.FieldName == nameof(Customer.Id)),
                Mock.Of<IFieldSelectionItem>(item => item.Alias == "acctNum" && item.FieldName == nameof(Customer.AccountNumber))
            };

            (selectionSet?.Selections).Should().BeEquivalentTo(expectedSelections);
        }
    }

    public class AddFieldToSelectionSetTestCase<T> where T : class
    {
        public AddFieldToSelectionSetTestCase(Action<ISelectionSetBuilder<T>> fieldAddition, string description)
        {
            FieldAddition = fieldAddition;
            Description = description;
        }

        public Action<ISelectionSetBuilder<T>> FieldAddition { get; }

        public string Description { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return Description;
        }
    }
}