using FluentAssertions;
using GraphQLQueryBuilder.Abstractions;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.SelectionSetBuilderTests
{
    public class When_Adding_A_Field
    {
        public static IEnumerable<AddFieldToSelectionSetTestCase<Customer>> NullPropertyExpressions
        {
            get
            {
                yield return new AddFieldToSelectionSetTestCase<Customer>(
                    builder => builder.AddField<Guid>(null),
                    "Adding a null property expression."
                );
                yield return new AddFieldToSelectionSetTestCase<Customer>(
                    builder => builder.AddField<Guid>("foo", null),
                    "Adding a null property expression."
                );
            }
        }

        [TestCaseSource(nameof(NullPropertyExpressions))]
        public void If_Property_Expression_Is_Null_Then_ArgumentNullException_Is_Thrown(
            AddFieldToSelectionSetTestCase<Customer> testData)
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Invoking(() => testData.FieldAddition(builder)).Should().ThrowExactly<ArgumentNullException>();
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
                .WithMessage($"When selecting a property that is a class, please use the {nameof(builder.AddCollectionField)} method that takes an {nameof(ISelectionSet)}.");
        }

        public static IEnumerable<AddFieldToSelectionSetTestCase<Customer>> FieldIsNotAMemberExpression
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

        [TestCaseSource(nameof(FieldIsNotAMemberExpression))]
        public void If_Expression_Is_Not_A_Member_Expression_Then_An_ArgumentException_Is_Thrown(
            AddFieldToSelectionSetTestCase<Customer> testData)
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Invoking(() => testData.FieldAddition(builder)).Should().ThrowExactly<ArgumentException>()
                .WithMessage($"A {nameof(MemberExpression)} was not provided.*")
                .Where(e => e.ParamName == "expression");
        }

        public static IEnumerable<AddFieldToSelectionSetTestCase<Customer>> FieldIsNotAMemberOfType
        {
            get
            {
                yield return new AddFieldToSelectionSetTestCase<Customer>(
                    builder => builder.AddField(customer => customer.CustomerContact.FirstName),
                    "Adding a field from a nested object that isn't on the model."
                );
                yield return new AddFieldToSelectionSetTestCase<Customer>(
                    builder => builder.AddField("foo", customer => customer.CustomerContact.FirstName),
                    "Adding a field using an alias from a nested object that isn't on the model."
                );
            }
        }

        [TestCaseSource(nameof(FieldIsNotAMemberOfType))]
        public void If_Expression_Is_Not_A_Property_Of_The_Class_Then_An_InvalidOperationException_Is_Thrown(
            AddFieldToSelectionSetTestCase<Customer> testData)
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Invoking(() => testData.FieldAddition(builder)).Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"Property '*' is not a member of type '{typeof(Customer).FullName}'.");
        }

        [InvalidAliasNamesTestCaseSource]
        public void If_Property_Alias_Is_Not_A_Valid_GraphQL_Name_Then_ArgumentException_Is_Thrown_Stating_Alias_Is_Not_Valid(
            string alias, string because)
        {
            var builder = SelectionSetBuilder.Of<Customer>();

            Action method = () => builder.AddField(alias, customer => customer.Id);

            Invoking(method).Should().ThrowExactly<ArgumentException>(because)
                .Where(e => e.ParamName == "alias", "because the alias parameter is the problem")
                .Where(e => e.HelpLink == "https://spec.graphql.org/June2018/#Name", "because this is the link to the GraphQL 'Name' spec");
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
        /// <remarks>Overriding so that the description is rendered in test outputs.</remarks>
        public override string ToString()
        {
            return Description;
        }
    }
}