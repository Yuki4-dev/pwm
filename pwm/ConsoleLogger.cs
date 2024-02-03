#nullable enable

using pwm.Core.API;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace pwm
{
    public class ConsoleLogger : ILogger
    {
        public void Error(string message = "", [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            Write(nameof(Error), message, path, name, line);
        }

        public void Info(string message = "", [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
#if DEBUG
            Write(nameof(Info), message, path, name, line);
#endif
        }

        public void Warn(string message = "", [CallerFilePath] string path = "", [CallerMemberName] string name = "", [CallerLineNumber] int line = 0)
        {
            Write(nameof(Warn), message, path, name, line);
        }

        private void Write(string label, string message, string path, string name, int line)
        {
            Debug.WriteLine($"[{label}]{DateTime.Now} : {message} (Method : {name}, line : {line}, path : {path} )");
        }
    }
}
