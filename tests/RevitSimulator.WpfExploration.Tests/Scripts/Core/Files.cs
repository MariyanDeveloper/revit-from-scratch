using System.IO;

namespace RevitSimulator.WpfExploration.Tests.Scripts.Core;

public static class Files
{
    public static File FromCSharpFile(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            throw new FileNotFoundException("File not found", path);
        }

        if (!Path.GetExtension(path).Equals(".cs"))
        {
            throw new FileNotFoundException("File not found", path);
        }
        return new File(path);
    }
}