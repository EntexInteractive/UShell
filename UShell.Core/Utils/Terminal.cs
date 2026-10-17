using System.Runtime.InteropServices;
using System.Text;

namespace UShell.Core.Utils
{
    public enum OperatingSystem
    {
        Any,
        Windows,
        Linux,
        MacOS
    }
    
    /// <summary>
    /// Represents a series of methods for cross-platform command line usage. This class can not be inherited.
    /// </summary>
    public static class Terminal
    {
        private static readonly object _consoleLock = new();

        public static bool IsDebugging = false;
        
        public static OperatingSystem Platform =>
            System.OperatingSystem.IsWindows() ? OperatingSystem.Windows :
            System.OperatingSystem.IsLinux() ? OperatingSystem.Linux :
            System.OperatingSystem.IsMacOS() ? OperatingSystem.MacOS :
            throw new PlatformNotSupportedException();

        public static void Write(object value, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.ForegroundColor = color;
            Console.Write($"\r> {value?.ToString()?.PadRight(Console.WindowWidth - 2)}");
            Console.ResetColor();
        }

        public static void WriteLine(object value, ConsoleColor color = ConsoleColor.Gray)
        {
            lock (_consoleLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(value);
                Console.ResetColor();
            }
        }

        public static void WriteArray(string message, IEnumerable<object> elements, int limit = 100)
        {
            lock (_consoleLock)
            {
                object[] array = elements.Order().ToArray();
                int display = Math.Min(limit, elements.Count());
                if (display > 0)
                {
                    WriteLine($" {message} ({elements.Count():N0}):");
                    for (int i = 0; i < display; i++)
                    {
                        WriteLine($"   {array.ElementAt(i)}");
                    }
                    
                    int remaining = elements.Count() - display;
                    if (remaining > 0)
                    {
                        WriteLine($"{remaining:N0} entr{(remaining == 1 ? "y" : "ies")} remaining...", ConsoleColor.DarkGray);
                    }
                }
                else
                {
                    WriteLine($" {message}: Empty");
                }
            }
        }
        
        public static void Progress(string message, double part, double total, TimeSpan elapsed)
        {
            if (part <= 0 || IsDebugging) return;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            
            double percent = part / total;
            string percentStr = $"> {message}: {Convert.ToInt32(percent * 100):D2}%";
            if (part >= total)
            {
                Console.ResetColor();
                Console.WriteLine();
            }

            double remainingMs = elapsed.TotalMilliseconds * (total - part) / part;
            TimeSpan remaining = remainingMs <= TimeSpan.MaxValue.TotalMilliseconds ? TimeSpan.FromMilliseconds(remainingMs) : TimeSpan.MaxValue;

            string remainingStr = $"{remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00} remaining";
            int barWidth = Math.Max(message.Length + 1, Console.WindowWidth - percentStr.Length - remainingStr.Length - 4);
            int filledWidth = Convert.ToInt32(percent * barWidth);

            StringBuilder progressBar = new StringBuilder(barWidth);
            for (int i = 0; i < filledWidth; i++)
            {
                progressBar.Append('#');
            }

            for (int i = filledWidth; i < barWidth; i++)
            {
                progressBar.Append('.');
            }

            Console.Write($"\r{percentStr} [{progressBar.ToString()}] {remainingStr}");
        }
        
        public static void PrintError(object message)
        {
            if(!IsDebugging) WriteLine(message, ConsoleColor.Red);
            Log.Error(message.ToString()!);
        }
        
        public static void PrintError(Exception exception)
        {
            if(!IsDebugging) WriteLine(exception.Message, ConsoleColor.Red);
            Log.Error(exception.GetBaseException().ToString());
        }
        
        public static void PrintInfo(object message)
        {
            if(!IsDebugging) WriteLine(message, ConsoleColor.White);
            Log.Information(message.ToString()!);
        }
        
        public static void PrintSuccess(object message)
        {
            if(!IsDebugging) WriteLine(message, ConsoleColor.Green);
            Log.Information(message.ToString()!);
        }

        public static void PrintWarning(object message)
        {
            if(!IsDebugging) WriteLine(message, ConsoleColor.Yellow);
            Log.Warning(message.ToString()!);
        }
    }
}