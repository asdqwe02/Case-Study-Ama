using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace GFramework.Logger.EditorConsoleSink
{
    public static class EditorConsoleSinkExtensions
    {
        private const string DEFAULT_DEBUG_OUTPUT_TEMPLATE = "[{Level:u3}] {Message:lj}{NewLine}{Exception}";
        
        public static LoggerConfiguration EditorConsole(
            this LoggerSinkConfiguration sinkConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DEFAULT_DEBUG_OUTPUT_TEMPLATE,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (sinkConfiguration == null)
            {
                throw new ArgumentNullException(nameof(sinkConfiguration));
            }

            if (outputTemplate == null)
            {
                throw new ArgumentNullException(nameof(outputTemplate));
            }

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            return sinkConfiguration.Sink(new EditorConsoleLogEventSink(formatter), restrictedToMinimumLevel, levelSwitch);
        }
    }
}