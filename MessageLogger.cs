namespace EliteSpawnTools
{
    using System;
    using VRage.Utils;

    public class MessageLogger
    {
        private const string Modname = "EliteSpawnTools";

        public void LogMessage(string message)
        {
            MyLog.Default.WriteLineAndConsole($"{Modname}: {message}");
        }

        public void LogException(Exception exception)
        {
            this.LogMessage($"{exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }
    }
}
