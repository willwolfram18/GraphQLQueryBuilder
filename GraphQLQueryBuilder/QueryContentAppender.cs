using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GraphQLQueryBuilder
{
    internal class QueryContentAppender
    {
        private const char IndentCharacter = ' ';
        private const uint IndentWidth = 4;

        private readonly uint _indentationLevel;
        private readonly StringBuilder _content;
        private readonly string _queryStart;

        internal QueryContentAppender(string queryName, uint indentationLevel)
        {
            _indentationLevel = indentationLevel;
            _queryStart = BuildIndentation(_indentationLevel) + queryName + " {";
            _content = new StringBuilder();
        }

        internal QueryContentAppender AppendProperty(string property)
        {
            AppendToContent(property);

            return this;
        }

        internal QueryContentAppender AppendChildQuery(string alias, QueryBuilder query)
        {
            var childQueryContent = query.Build(_indentationLevel + 1);

            AppendToContent(alias, childQueryContent);

            return this;
        }

        internal string Build()
        {
            var indentation = BuildIndentation(_indentationLevel);
            var result = new StringBuilder()
                .AppendLine(_queryStart);

            if (_content.Length != 0)
            {
                result.AppendLine(_content.ToString());
            }

            result.Append(indentation + "}");

            return result.ToString();
        }

        private void AppendToContent(string content)
        {
            AppendToContent(string.Empty, content);
        }

        private void AppendToContent(string alias, string content)
        {
            if (_content.Length != 0)
            {
                _content.Append(",\n");
            }

            var indentation = BuildIndentation(_indentationLevel + 1);
            var prefix = indentation;

            if (!string.IsNullOrWhiteSpace(alias))
            {
                prefix += $"{alias}: ";
            }

            if (content.StartsWith(indentation))
            {
                content = Regex.Replace(content, $"^{indentation}", prefix);
            }
            else
            {
                content = prefix + content;
            }

            _content.Append(content);
        }

        private string BuildIndentation(uint indentationLevel)
        {
            return new string(IndentCharacter, (int)(indentationLevel * IndentWidth));
        }
    }
}