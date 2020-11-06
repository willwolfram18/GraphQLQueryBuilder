using System.Text.RegularExpressions;

namespace GraphQLQueryBuilder.Implementations.Language
{
    internal static class GraphQLName
    {
        private static readonly Regex ValidName = new Regex(@"^[_A-Za-z][_0-9A-Za-z]*$");

        public static bool IsValid(string name)
        {
            return ValidName.IsMatch(name);
        }
    }
}
