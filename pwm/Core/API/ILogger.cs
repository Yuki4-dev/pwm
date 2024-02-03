#nullable enable

using System.Runtime.CompilerServices;

namespace pwm.Core.API
{
    public interface ILogger
    {
        void Info(string message = "", [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0);
        void Warn(string message = "", [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0);
        void Error(string message = "", [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0);
    }
}
