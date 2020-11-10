using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Primitives;
using GraphQLQueryBuilder.Abstractions.Language;
using GraphQLQueryBuilder.Tests.Models;
using NUnit.Framework;

namespace GraphQLQueryBuilder.Tests.ArgumentBuilderTests
{
    [TestFixtureSource(nameof(CustomerStatuses))]
    public class When_Building_An_Enum_Argument_Value : ArgumentBuilderTest<CustomerStatus, IEnumArgumentValue<CustomerStatus>>
    {
        public When_Building_An_Enum_Argument_Value(CustomerStatus status)
        {
            ArgumentValue = status;
        }

        public static IEnumerable<CustomerStatus> CustomerStatuses =>
            Enum.GetValues(typeof(CustomerStatus)).Cast<CustomerStatus>();
        
        protected override CustomerStatus ArgumentValue { get; set; }

        protected override IArgument BuildArgument(string name, CustomerStatus value) =>
            ArgumentBuilder.Build(name, value);

        protected override void ValidateArgumentValue(AndWhichConstraint<ObjectAssertions, IEnumArgumentValue<CustomerStatus>> argumentValueAssertions)
        {
            argumentValueAssertions.Which.Value.Should().Be(ArgumentValue);
            argumentValueAssertions.Which.Should().BeAssignableTo<IEnumArgumentValue>()
                .Which.Value.Should().Be(ArgumentValue);
        }
    }
}