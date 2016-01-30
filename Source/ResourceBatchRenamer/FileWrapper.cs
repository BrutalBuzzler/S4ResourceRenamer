using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ResourceBatchRenamer
{
    [ExcludeFromCodeCoverage]
    public class FileWrapper : IFile
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }
    }
}