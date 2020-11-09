using System;
using System.Linq;
using FluentAssertions;
using GraphQLQueryBuilder.Implementations.Language;
using NUnit.Framework;
using static FluentAssertions.FluentActions;

namespace GraphQLQueryBuilder.Tests.ArgumentListTests
{
    public class When_Adding_An_Argument
    {
        [Test]
        public void Then_An_Argument_Cannot_Be_Duplicated()
        {
            var argument = ArgumentBuilder.Build("a", 1);
            var duplicate = ArgumentBuilder.Build(argument.Name, "will fail");

            var systemUnderTest = new ArgumentList
            {
                argument
            };

            Invoking(() => systemUnderTest.Add(duplicate)).Should().ThrowExactly<InvalidOperationException>()
                .WithMessage($"There can be only one argument named '{argument.Name}'.");
        }

        [Test]
        public void Then_All_Arguments_Are_In_The_Collection()
        {
            var expectedArguments = new[]
            {
                ArgumentBuilder.Build("foo", "bloop"),
                ArgumentBuilder.Build("bar", 13),
                ArgumentBuilder.Build("baz", 84.19),
                ArgumentBuilder.Build("foobar"),
                ArgumentBuilder.Build("jorb", false)
            };

            var systemUnderTest = new ArgumentList();

            foreach (var arg in expectedArguments)
            {
                systemUnderTest.Add(arg);
            }

            systemUnderTest.Should().BeEquivalentTo(expectedArguments.ToList());
        }

        [Test]
        public void Then_Null_Argument_Instances_Are_Not_Allowed()
        {
            var systemUnderTest = new ArgumentList();

            Invoking(() => systemUnderTest.Add(null)).Should().ThrowExactly<ArgumentNullException>()
                .Which.ParamName.Should().Be("item");
        }
    }
}