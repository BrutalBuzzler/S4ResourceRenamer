using System;
using System.Collections.Generic;
using System.Linq;

namespace ResourceBatchRenamer
{
    public class FileRenamer
    {
        private readonly IFile file;
        private readonly FileNameConverter fileNameConverter;
        private readonly IEnumerable<string> files;

        public FileRenamer(IFile file, FileNameConverter fileNameConverter, string[] files)
        {
            GuardNotNull(file, "file");
            GuardNotNull(fileNameConverter, "fileNameConverter");
            GuardNotNullOrEmpty(files);

            this.file = file;
            this.fileNameConverter = fileNameConverter;
            this.files = files;
        }

        public void Rename()
        {
            Dictionary<string, string> nameMap = this.GetSourceToTargetNamesMap();
            foreach (var namePair in nameMap)
            {
                this.GuardFileExists(namePair.Key);
                this.file.Move(namePair.Key, namePair.Value);
            }
        }

        private Dictionary<string, string> GetSourceToTargetNamesMap()
        {
            var nameMap = this.files.ToDictionary(s => s, this.fileNameConverter.ConvertToPackageManagerConvention);
            return nameMap;
        }

        private void GuardFileExists(string path)
        {
            if (!this.file.Exists(path))
            {
                throw new ArgumentException(string.Format("File '{0}' not found.", path));
            }
        }

        private static void GuardNotNullOrEmpty(string[] files)
        {
            GuardNotNull(files, "files");
            if (!files.Any())
            {
                throw new ArgumentException("files must not be empty");
            }
        }

        // ReSharper disable once UnusedParameter.Local
        private static void GuardNotNull(object value, string name)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentException(name);
            }
        }
    }
}