// Copyright 2026 Entex Interactive

namespace UShell.Core.Utils
{
    /// <summary>
    /// Represents a way of interacting with a system file. This class can not be inherited.
    /// </summary>
    public class FileSystemUtility
    {
        /// <summary>
        /// Gets the directory path used for storing application-specific settings.
        /// Automatically ensures that the directory exists within the user's ApplicationData folder.
        /// </summary>
        public string SettingsDirectory
        {
            get
            {
                string settingsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "UShell");
                if (!Directory.Exists(settingsDir)) Directory.CreateDirectory(settingsDir);
                return settingsDir;
            }
        }

        /// <summary>
        /// Gets the directory path used for storing temporary files.
        /// Automatically ensures that the directory exists within the system's temporary folder.
        /// </summary>
        public string TempDirectory
        {
            get
            {
                string tempDir = Path.Combine(Path.GetTempPath(), "UShell");
                if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                return tempDir;
            }
        }

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