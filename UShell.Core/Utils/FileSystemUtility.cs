// Copyright 2026 Entex Interactive

namespace UShell.Core.Utils
{
    /// <summary>
    /// Represents a way of interacting with a system file. This class can not be inherited.
    /// </summary>
    public class FileSystemUtility
    {
        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) return;
            DirectoryInfo dir = new DirectoryInfo(path);
            Log.Debug("Deleting: {Path}", path);

            foreach (FileInfo file in dir.GetFiles("*", SearchOption.AllDirectories))
            {
                file.IsReadOnly = false;
            }

            foreach (DirectoryInfo subDir in dir.GetDirectories("*", SearchOption.AllDirectories))
            {
                subDir.Attributes = FileAttributes.Normal;
            }

            dir.Attributes = FileAttributes.Normal;
            dir.Delete(true);
        }
    }
}