namespace ResourceBatchRenamer.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class FileNameConverterTests
    {
        private FileNameConverter renamer;

        [SetUp]
        public void Setup()
        {
            this.renamer = new FileNameConverter();
        }

        [TestCase(false, @"")]
        [TestCase(false, null)]
        [TestCase(true, @"0x00000000!0x07a00296298d1234.trayitem")]
        [TestCase(true, @"0x00001234!0x07a00296298c0116.blueprint")]
        [TestCase(true, @"0x00001234!0x07a00296298c0116.bpi")]
        [TestCase(true, @"0x00001234!0x07a00296298c0116.householdbinary")]
        [TestCase(true, @"0x00001234!0x07a00296298c0116.hhi")]
        [TestCase(true, @"0x00001234!0x07a00296298c0116.sgi")]
        [TestCase(true, @"0x00001234!0x07a00296298c0116.room")]
        [TestCase(true, @"0x0000A234!0x07a00296298c0116.rmi")]
        [TestCase(false, @"0x0000A234-0x07a00296298c0116.rmi")]
        [TestCase(false, @"0x0000z234!0x07a00296298c0116.rmi")]
        public void IsValidFileNameShouldWorkProperly(bool isValid, string fileName)
        {
            bool actual = FileNameConverter.IsValidFileName(fileName);

            Assert.AreEqual(isValid, actual);
        }

        [Test]
        public void ConvertToPackageManagerConventionThrowOnInvalidFile()
        {
            const string input = @"0x00000000_0x07a00296298d1234_trayitem";

            Assert.Catch<ArgumentException>(() => this.renamer.ConvertToPackageManagerConvention(input));
        }

        [TestCase(@"D:\temp\0x00000000!0x07a00296298d1234.trayitem", @"D:\temp\S4_0x2a8a5e22_0x00000000_0x07a00296298d1234.trayitem")]
        [TestCase(@"C:\something\0x00001234!0x07a00296298c0116.blueprint", @"C:\something\S4_0x3924de26_0x00001234_0x07a00296298c0116.blueprint")]
        [TestCase(@"0x00001234!0x07a00296298c0116.blueprint", @"S4_0x3924de26_0x00001234_0x07a00296298c0116.blueprint")]
        public void FromTs4ToS4PeConventionShouldReorderPartsCorrectly(string value, string expected)
        {
            string actual = this.renamer.ConvertToPackageManagerConvention(value);

            Assert.AreEqual(expected, actual);
        }

        [TestCase("trayitem", "0x2a8a5e22")]
        [TestCase("blueprint", "0x3924de26")]
        [TestCase("bpi", "0xd33c281e")]
        [TestCase("householdbinary", "0xb3c438f0")]
        [TestCase("hhi", "0x3bd45407")]
        [TestCase("sgi", "0x56278554")]
        [TestCase("room", "0x370efd6e")]
        [TestCase("rmi", "0x00de5ac5")]
        public void ConvertToPackageManagerConventionShouldMapNamesToTypeValues(string typeName, string typeValue)
        {
            string input = @"0x00001234!0x07a00296298c0116." + typeName;
            string expected = string.Format(@"S4_{0}_0x00001234_0x07a00296298c0116.{1}", typeValue, typeName);

            Assert.AreEqual(expected, this.renamer.ConvertToPackageManagerConvention(input));
        }

        [Test]
        public void ConvertToPackageManagerConventionShouldThrowOnUnsupportedType()
        {
            string input = @"0x00001234!0x07a00296298c0116.unsupported";

            Assert.Catch<NotSupportedException>(() => this.renamer.ConvertToPackageManagerConvention(input));
        }
    }
}