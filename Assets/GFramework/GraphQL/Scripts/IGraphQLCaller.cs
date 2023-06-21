using System.Collections;

namespace GFramework.GraphQL
{
    public interface IGraphQLCaller
    {
        IEnumerator CallQueryAsync(GraphQLRequest request);
    }
}