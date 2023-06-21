using JetBrains.Annotations;
using Newtonsoft.Json.Linq;

namespace GFramework.GraphQL
{
    public class GraphQLQueryResult
    {
        public GraphQLQueryResult(long responseCode, [CanBeNull] dynamic response)
        {
            ResponseCode = responseCode;
            Response = response;
        }

        public long ResponseCode
        {
            get;
        }

        public dynamic Response
        {
            get;
        }
        

        public bool IsHttpError => ResponseCode != 200;
        public bool IsGraphQLError => Response.errors != null;

        public JArray Errors => Response.errors;
        
        public dynamic Data => Response.data;
    }
}