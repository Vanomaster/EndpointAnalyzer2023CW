using System.Reflection;

namespace Common;

/// <summary>
/// Application instance folder helper.
/// </summary>
public static class InstanceFolderHelper
{
    /// <summary>
    /// Get application instance root folder (instance.info location).
    /// </summary>
    /// <returns>Application instance root folder.</returns>
    public static DirectoryInfo GetInstanceFolder()
    {
        // var instanceFolder = Directory.GetParent(GetAssemblyDirectoryPath());
        var instanceFolder = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (instanceFolder != null && !File.Exists(Path.Combine(instanceFolder.FullName, "instance.info")))
        {
            instanceFolder = instanceFolder.Parent;
        }

        if (instanceFolder == null)
        {
            throw new Exception("Cannot found instance.info.");
        }

        return instanceFolder;
    }

    private static string GetAssemblyDirectoryPath()
    {
        string location = Assembly.GetExecutingAssembly().Location;
        var uri = new UriBuilder(location);
        string path = Uri.UnescapeDataString(uri.Path);
        string assemblyDirectoryPath = Path.GetDirectoryName(path) ?? Directory.GetCurrentDirectory();

        return assemblyDirectoryPath;
    }
}