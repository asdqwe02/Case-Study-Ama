using System.Collections;
using System.Collections.Generic;
using GFramework.Data;
using GFramework.Examples.Signals;
using GFramework.GraphQL;
using GFramework.Runner;
using JetBrains.Annotations;
using Zenject;
using ILogger = GFramework.Logger.ILogger;

namespace GFramework.Examples
{
    [UsedImplicitly]
    public class ExampleScene1Logic
    {
        [Inject] private ILogger _logger;
        [Inject] private IRunner _runner;
        [Inject] private IDataManager _dataManager;
        [Inject] private SignalBus _signalBus;

        [Inject] private IGraphQLCaller _graphQLCaller;

        // this is not the same as Unity "Start" it's just a function in a class you can call it anywhere
        public void Start()
        {
            //Logger Demo
            _logger.Verbose("Verbose Log");
            _logger.Debug("Debug");
            _logger.Information("Loading in {@LoadingTime} seconds...", 3);
            _logger.Warning("Warning");
            _logger.Error("Error");
            //Runner Demo
            _runner.CallOnMainThread(() => { _logger.Information("Hello"); });

            _runner.StartCoroutine(CallQuery());
            // Signal Bus  Demo
            _signalBus.Fire(new ExampleSignal
            {
                Message = "Example Signal Test"
            });
            // Data Manager Demo
            _dataManager.Save("Test", new TestClass());
        }

        private IEnumerator CallQuery()
        {
            var request = new GraphQLRequest
            {
                QueryName = GraphQL.LOGIN,
                Parameters = new Dictionary<string, object>
                {
                    { "username", "admin1234" },
                    { "password", "12345678" },
                },
            };

            request.OnComplete += result =>
            {
                if (result.IsHttpError)
                {
                    _logger.Information(result.ResponseCode.ToString());
                    _logger.Information(result.Response);
                }
                else
                {
                    if (result.IsGraphQLError)
                    {
                        _logger.Information(result.Errors.ToString());
                        foreach (var error in result.Errors)
                        {
                            _logger.Information(error.ToString());
                        }
                    }
                    else
                    {
                        _logger.Information(result.Data.ToString());
                    }
                }
            };
            yield return _graphQLCaller.CallQueryAsync(request);
        }
    }
}