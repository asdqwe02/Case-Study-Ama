using UnityEngine;

namespace GFramework.Logger
{
    [CreateAssetMenu(fileName = "LogConfig.asset", menuName = "GFramework/Settings/Log Config")]
    public class LogConfig : ScriptableObject
    {
        [Header("Editor Console")]
        [SerializeField]
        private bool _enableEditorConsoleLogging;

        public bool EnableEditorConsoleLogging => _enableEditorConsoleLogging;

        [SerializeField]
        private LogLevel _minimumEditorConsoleLogLevel;

        public LogLevel MinimumEditorConsoleLogLevel => _minimumEditorConsoleLogLevel;

        [Header("File")]
        [SerializeField]
        private bool _enableFileLogging;

        public bool EnableFileLogging => _enableFileLogging;

        [SerializeField]
        private LogLevel _minimumFileLogLevel;

        public LogLevel MinimumFileLogLevel => _minimumFileLogLevel;

        [Header("Colors")]
        [SerializeField]
        private Color _debugInfoLogColor; 

        public Color32 InfoLogColor => _debugInfoLogColor;

        [SerializeField]
        private Color _warningLogColor;

        public Color32 WarningLogColor => _warningLogColor;

        [SerializeField]
        private Color _errorLogColor;

        public Color32 ErrorLogColor => _errorLogColor;
    }
}