using UnityEngine;

public class FolderPathAttribute : PropertyAttribute
{
    public readonly string DefaultPath;

    public FolderPathAttribute(string defaultPath = "")
    {
        DefaultPath = defaultPath;
    }
}