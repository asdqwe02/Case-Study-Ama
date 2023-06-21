using System.Collections;
using System.Linq;
using System.Text;
using GFramework.GraphQL.Exceptions;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace GFramework.GraphQL
{
    public class GGraphQLCaller: MonoBehaviour, IGraphQLCaller
    {
        [Inject]
        private GraphQLConfig _config;


        public IEnumerator CallQueryAsync(GraphQLRequest request)
        {
            var query = _config.Queries.FirstOrDefault(q => q.QueryName == request.QueryName);
            if (query == null)
            {
                throw new GraphQLQueryNotFoundException();
            }

            var queryString = query.Query;
            var variableString = query.Variables;
            foreach (var param in request.Parameters)
            {
                switch (param.Value)
                {
                    case int:
                    case float:
                        queryString = queryString.Replace($"$[{param.Key}]", param.Value.ToString());
                        variableString = variableString.Replace($"$[{param.Key}]", param.Value.ToString());

                        break;
                    case string:
                        queryString = queryString.Replace($"$[{param.Key}]", $"\"{param.Value}\"");
                        variableString = variableString.Replace($"$[{param.Key}]", $"\"{param.Value}\"");

                        break;
                }
            }

            queryString = queryString.Replace("\r\n", "\\n")
                .Replace("\n", "\\n")
                .Replace("\"", "\\\"");
                    
            var body = $"{{\"query\":\"{queryString}\",\"variables\":{variableString},\"operationName\":null}}";
            Debug.Log(body);
            var downloadHandler = new DownloadHandlerBuffer();
            var webRequest = new UnityWebRequest(_config.Host, "POST");

            var bodyRaw = Encoding.UTF8.GetBytes(body);
            webRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            
            webRequest.downloadHandler = downloadHandler;
            
            webRequest.SetRequestHeader("Content-Type", "application/json");
            foreach (var header in request.Headers)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }

            yield return webRequest.SendWebRequest();

            if (webRequest.responseCode == 200)
            {
                dynamic response = JObject.Parse(downloadHandler.text);
                request.OnComplete?.Invoke(new GraphQLQueryResult(webRequest.responseCode, response));
            }
            else
            {
                request.OnComplete?.Invoke(new GraphQLQueryResult(webRequest.responseCode, null));
            }
                    
            downloadHandler.Dispose();
            webRequest.uploadHandler?.Dispose();
            webRequest.Dispose();
        }
    }
}