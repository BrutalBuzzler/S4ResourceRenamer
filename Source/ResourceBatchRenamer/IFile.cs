namespace ResourceBatchRenamer
{
    public interface IFile
    {
        bool Exists(string path);

        void Move(string sourceFileName, string destFileName);
    }
}