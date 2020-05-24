using NUnit.Framework;
using Snapshooter.Json;
using System;
using System.Runtime.CompilerServices;

namespace GraphQLQueryBuilder.Tests
{
    [TestFixture]
    public abstract class TestClass
    {
        protected const string DuplicateFragmentSkipReason = "Determine duplicate fragment resolution.";

        protected void ResultMatchesSnapshotOfMatchingClassAndTestName(string result, [CallerMemberName] string methodName = "")
        {
            Snapshot.Match(result, GenerateSnapshotNameFromClassAndTestNames(methodName));
        }

        private string GenerateSnapshotNameFromClassAndTestNames(string methodName)
        {
            var typeOfThisTest = GetType();
            var methodInfo = typeOfThisTest.GetMethod(methodName) ??
                             throw new InvalidOperationException($"Did not find method {methodName} on type {typeOfThisTest.FullName}");

            return $"{typeOfThisTest.Name}.{methodInfo.Name}";
        }
    }
}
