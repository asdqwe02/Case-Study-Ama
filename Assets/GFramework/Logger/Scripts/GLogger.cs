using System;
using GFramework.Logger.EditorConsoleSink;
using JetBrains.Annotations;
using Serilog;
using UnityEngine;

// ReSharper disable TemplateIsNotCompileTimeConstantProblem

namespace GFramework.Logger
{
    [UsedImplicitly]
    public class GLogger: ILogger
    {
        private readonly Serilog.Core.Logger _editorConsoleLogger;
        private readonly Serilog.Core.Logger _fileLogger;

        public GLogger(LogConfig config)
        {
#if !UNITY_EDITOR
            UnityEngine.Debug.unityLogger.logEnabled = false;
#endif
            if (config.EnableFileLogging)
            {
                _fileLogger = CreateLogConfig(config.MinimumFileLogLevel).
                    WriteTo.File($"{Application.persistentDataPath}/log.txt", 
                    rollingInterval:RollingInterval.Day,
                    rollOnFileSizeLimit:true,
                    fileSizeLimitBytes:1024).CreateLogger();
            }

            if (config.EnableEditorConsoleLogging)
            {
                _editorConsoleLogger = CreateLogConfig(config.MinimumEditorConsoleLogLevel).
                    WriteTo.EditorConsole().CreateLogger();
            }

            
            Application.quitting += () =>
            {
                _fileLogger?.Dispose();
                _editorConsoleLogger?.Dispose();
            };
        }

        private static LoggerConfiguration CreateLogConfig(LogLevel minimumLevel)
        {
            var loggerConfig = new LoggerConfiguration();
            loggerConfig = minimumLevel switch
            {
                LogLevel.VERBOSE => loggerConfig.MinimumLevel.Verbose(),
                LogLevel.INFORMATION => loggerConfig.MinimumLevel.Information(),
                LogLevel.DEBUG => loggerConfig.MinimumLevel.Debug(),
                LogLevel.WARNING => loggerConfig.MinimumLevel.Warning(),
                LogLevel.ERROR => loggerConfig.MinimumLevel.Error(),
                LogLevel.FATAL => loggerConfig.MinimumLevel.Fatal(),
                _ => throw new ArgumentOutOfRangeException(),
            };
            return loggerConfig;
        }

        public void Verbose(string message, params object[] propertyValues)
        {
            _fileLogger?.Verbose(message, propertyValues);
            _editorConsoleLogger?.Verbose(message, propertyValues);
        }

        public void Debug(string message, params object[] propertyValues)
        {
            _fileLogger?.Debug(message, propertyValues);
            _editorConsoleLogger?.Debug(message, propertyValues);
        }

        public void Information(string message, params object[] propertyValues)
        {
            _fileLogger?.Information(message, propertyValues);
            _editorConsoleLogger?.Information(message, propertyValues);
        }

        public void Warning(string message, params object[] propertyValues)
        {
            _fileLogger?.Warning(message, propertyValues);
            _editorConsoleLogger?.Warning(message, propertyValues);
        }

        public void Error(string message, params object[] propertyValues)
        {
            _fileLogger?.Error(message, propertyValues);
            _editorConsoleLogger?.Error(message, propertyValues);
        }

        public void Fatal(string message, params object[] propertyValues)
        {
            _fileLogger?.Fatal(message, propertyValues);
            _editorConsoleLogger?.Fatal(message, propertyValues);
        }
    }
}