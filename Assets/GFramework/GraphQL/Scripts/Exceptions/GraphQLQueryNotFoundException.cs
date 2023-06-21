using System;

namespace GFramework.GraphQL.Exceptions
{
    public class GraphQLQueryNotFoundException: Exception
    {
        public GraphQLQueryNotFoundException() : base("Graph QL Query not found")
        {
        }
    }
}