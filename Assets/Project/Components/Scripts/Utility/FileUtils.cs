using System;
using UnityEngine;

namespace Project.Components.Scripts.Utility
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
            int index = path.IndexOf(projectFolderName, StringComparison.OrdinalIgnoreCase);

            if (index >= 0)
            {
                path = path.Substring(index);
            }
            else
            {
                Debug.Log($"Selected path does not contain {projectFolderName}. Folder path equals full path.");
            }

            return path;
        }
    }
}