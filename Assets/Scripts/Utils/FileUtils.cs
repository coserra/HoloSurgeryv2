using System;
using System.IO;

public static class FileUtils
{
    //From: https://stackoverflow.com/questions/680786/rename-some-files-in-a-folder
    public static void Rename(this FileInfo file, string newName)
    {
        // Validate arguments.
        if (file == null)
        {
            throw new ArgumentNullException("file");
        }
        else if (newName == null)
        {
            throw new ArgumentNullException("newName");
        }
        else if (newName.Length == 0)
        {
            throw new ArgumentException("The name is empty.", "newName");
        }
        else if (newName.IndexOf(Path.DirectorySeparatorChar) >= 0
            || newName.IndexOf(Path.AltDirectorySeparatorChar) >= 0)
        {
            throw new ArgumentException("The name contains path separators. The file would be moved.", "newName");
        }

        // Rename file.
        string newPath = Path.Combine(file.DirectoryName, newName);
        file.MoveTo(newPath);
    }
}
