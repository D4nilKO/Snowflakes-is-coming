using System;
using UnityEngine;

namespace Project.Utility
{
    public class FileUtils
    {
        public static void RemoveLocalPath(ref string path, string projectFolderName = "Assets")
        {
            int index = path.IndexOf(projectFolderName, StringComparison.OrdinalIgnoreCase);

            if (index >= 0)
            {
                path = path.Substring(index);
            }
            else
            {
                Debug.Log($"Selected path does not contain {projectFolderName}. Folder path equals full path.");
            }
        }

        public static string RemoveLocalPath(string path, string projectFolderName = "Assets")
        {
            string newPath = path;
            RemoveLocalPath(ref newPath, projectFolderName);

            return newPath;
        }
    }
}