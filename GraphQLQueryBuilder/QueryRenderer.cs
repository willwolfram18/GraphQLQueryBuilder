using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQLQueryBuilder.Abstractions.Language;

namespace GraphQLQueryBuilder
{
    public class QueryRenderer : IQueryRenderer
    {
        /// <inheritdoc />
        public string Render(ISelectionSet selectionSet)
        {
            return "fake";
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public string Render(IGraphQLOperation query)
        {
            throw new NotImplementedException();
        }
    }
}
