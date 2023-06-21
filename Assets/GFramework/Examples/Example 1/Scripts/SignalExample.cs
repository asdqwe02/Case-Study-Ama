using System;
using GFramework.Examples.Signals;
using UnityEngine;
using Zenject;
using ILogger = GFramework.Logger.ILogger;

namespace GFramework.Examples
{
    public class SignalExample : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private ILogger _logger;

        private void Awake()
        {
            _signalBus.Subscribe<ExampleSignal>(OnExampleSignalRecieved);
        }

        private void OnExampleSignalRecieved(ExampleSignal obj)
        {
            _logger.Information($"Recieved Example Signal with message: {obj.Message}");
        }
    }
}