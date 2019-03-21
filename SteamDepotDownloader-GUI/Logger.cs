using System;
using System.Text;

namespace DepotDownloader
{
    internal static class Logger
    {
        static StringBuilder  _sBuilder = new StringBuilder(128);
        public static void Info(string line, params object[] args)
        {
            //_Config.FireOnMessageEvent("log", Format(line, args));
            Console.WriteLine("Info:"+Format(line,args));
        }
        
        public static void Warning(string line, params object[] args)
        {
            //_Config.FireOnMessageEvent("warning", Format(line, args));
            Console.WriteLine("Warning:" + Format(line, args));
        }
        
        public static void Error(string line, params object[] args)
        {
            //_Config.FireOnMessageEvent("error", Format(line, args));
            Console.WriteLine("Error:" + Format(line, args));
        }
        
        public static void Exception(Exception exception)
        {
            //_Config.FireOnMessageEvent("exception", exception);
        }

        public static string Format(string line, params object[] args)
        {
            _sBuilder.Append(line);
            for (var i = 0; i < args.Length; i++)
            {
                _sBuilder.Replace($"{{{i}}}", $"{args[i]}");
            }
            
            line = _sBuilder.ToString();
            _sBuilder.Clear();
            return line;
        }
    }
}