using System.Runtime.InteropServices;
using System.Text;

namespace UShell.Core.Utils
{
    public enum OperatingSystem
    {
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
                Console.WriteLine($"> {value}");
                Console.ResetColor();
            }
        }

        public static void WriteArray(string message, IEnumerable<string> elements)
        {
            lock (_consoleLock)
            {
                string[] array = elements.Order().ToArray();
                if (array.Length > 0)
                {
                    Console.WriteLine($"> {message}:");
                    foreach (string item in array)
                    {
                        Console.WriteLine($">   - {item}");
                    }
                }
                else
                {
                    Console.WriteLine($"> {message}: 0");
                }
            }
        }
        
        public static void Progress(double part, double total, TimeSpan elapsed)
        {
            double percent = part / total;
            string percentStr = $"> Progress: {Convert.ToInt32(percent * 100):D2}%";

            TimeSpan remaining;
            double remainingMs = elapsed.TotalMilliseconds * (total - part) / part;
            if (remainingMs <= TimeSpan.MaxValue.TotalMilliseconds) remaining = TimeSpan.FromMilliseconds(remainingMs);
            else remaining = TimeSpan.MaxValue;

            string remainingStr = $"{remaining.Hours:00}:{remaining.Minutes:00}:{remaining.Seconds:00} remaining";
            int barWidth = Console.WindowWidth - percentStr.Length - remainingStr.Length - 4;
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
        
        public static void Error(object message)
        {
            WriteLine(message, ConsoleColor.Red);
            Log.Error(message.ToString()!);
        }
        
        public static void Error(Exception exception)
        {
            WriteLine(exception.Message, ConsoleColor.Red);
            Log.Error(exception.GetBaseException().ToString());
        }
        
        public static void Information(object message)
        {
            WriteLine(message, ConsoleColor.White);
            Log.Information(message.ToString()!);
        }
        
        public static void Success(object message)
        {
            WriteLine(message, ConsoleColor.Green);
            Log.Information(message.ToString()!);
        }

        public static void Warning(object message)
        {
            WriteLine(message, ConsoleColor.Yellow);
            Log.Warning(message.ToString()!);
        }
    }
}