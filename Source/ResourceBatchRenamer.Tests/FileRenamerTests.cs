using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace ResourceBatchRenamer.Tests
{
    [TestFixture]
    public class FileRenamerTests
    {
        private IFile file;
        private FileNameConverter converter;
        private static readonly string[] testFileNames = {"file1", "file2"};
        private FileRenamer fileRenamer;

        [SetUp]
        public void Setup()
        {
            this.file = MockRepository.GenerateStub<IFile>();
            this.converter = MockRepository.GeneratePartialMock<FileNameConverter>();

            this.fileRenamer = new FileRenamer(this.file, this.converter, testFileNames);
        }

        [Test]
        public void ConstructorShouldGuardFile()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Catch<ArgumentException>(() => new FileRenamer(null, this.converter, testFileNames));
        }
        
        [Test]
        public void ConstructorShouldGuardConverter()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Catch<ArgumentException>(() => new FileRenamer(this.file, null, testFileNames));
        }

        [Test]
        public void ConstructorShouldGuardFileNames()
        {
            // ReSharper disable ObjectCreationAsStatement
            Assert.Catch<ArgumentException>(() => new FileRenamer(this.file, this.converter, null));
            Assert.Catch<ArgumentException>(() => new FileRenamer(this.file, this.converter, new string[0]));
            // ReSharper restore ObjectCreationAsStatement
        }

        [Test]
        public void RenameShouldRenameFiles()
        {
            this.converter.Stub(c => c.ConvertToPackageManagerConvention(null))
                .IgnoreArguments()
                .Do(new Func<string, string>(s => "renamed_" + s));
            this.file.Stub(f => f.Exists(null)).IgnoreArguments().Return(true);

            this.fileRenamer.Rename();

            this.file.AssertWasCalled(f => f.Move(testFileNames[0], "renamed_" + testFileNames[0]));
            this.file.AssertWasCalled(f => f.Move(testFileNames[1], "renamed_" + testFileNames[1]));
        }

        [Test]
        public void RenameShouldThrowWhenFileDoesNotExist()
        {
            this.converter.Stub(c => c.ConvertToPackageManagerConvention(null)).IgnoreArguments().Return("file");
            this.file.Stub(f => f.Exists(null)).IgnoreArguments().Return(false);

            Assert.Catch<ArgumentException>(() => this.fileRenamer.Rename());
        }
    }
}