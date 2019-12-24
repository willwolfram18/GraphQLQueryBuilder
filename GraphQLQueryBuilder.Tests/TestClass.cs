using NUnit.Framework;
using System;
using System.Runtime.CompilerServices;

namespace GraphQLQueryBuilder.Tests
{
    [TestFixture]
    public abstract class TestClass
    {
        protected string GetSnapshotName([CallerMemberName] string methodName = "")
        {
            var typeOfThisTest = GetType();
            var methodInfo = typeOfThisTest.GetMethod(methodName) ??
                             throw new InvalidOperationException($"Did not find method {methodName} on type {typeOfThisTest.Name}");

            return $"{typeOfThisTest.Name}.{methodInfo.Name}";
        }
    }
}
