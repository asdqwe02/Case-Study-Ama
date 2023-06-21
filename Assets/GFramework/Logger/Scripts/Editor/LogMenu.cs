using GFramework.Core.Editor;
using JetBrains.Annotations;

namespace GFramework.Logger.Editor
{
    [UsedImplicitly]
    public class LogMenu: Menu<LogConfig>
    {
        public override string MenuName => "Settings/Logger";
    }
}