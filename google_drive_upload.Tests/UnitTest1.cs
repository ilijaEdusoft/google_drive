using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;
namespace google_drive_upload.Tests
{
    public class FileNameTest
    {
        Uploader up = new Uploader();
        [Theory]
        [InlineData("Hel1oWo231.xlsx", true)]
        [InlineData("HelloWorld.docx", true)]
        [InlineData("H64430el1oWo23A.pdf", true)]
        [InlineData("test11233442.txt", true)]
        [InlineData("823920usjsjsGDGDJKJSG.xlsx", true)]
        [InlineData("2Rel@1#oWo!21.xlsx", false)]
        [InlineData("!H@e$1oWo23@1.docx", false)]
        [InlineData("1091@$(-19Djw.pdf", false)]
        [InlineData("@3392jw#^@(@)38392.txt", false)]
        [InlineData("!@!@!()#*(!#!)(#&*!#^#$%$#&*(!.pdf", false)]
        public void checkFileName(string input, bool expected)
        {
            string? reg = Uploader._regex;
            var r = new Regex(reg, RegexOptions.IgnoreCase);
            Assert.Equal(expected, r.Match(input).Success);
        }
        [Fact]
        public void IsFileExists()
        {
            var tempFilePath = Path.GetTempFileName();
            var file = new FileInfo(tempFilePath);
            Assert.True(file.Exists);
        }
        [Fact]
        public void CheckFileExists()
        {
            var tempFile = Path.GetTempFileName();
            bool exists = up.FileExists(tempFile);
            Assert.True(exists);
        }
        [Fact]
        public void CheckFileNotExists()
        {
            var tempFile = Path.GetTempFileName();
            File.Delete(tempFile);
            bool exists = up.FileExists(tempFile);
            Assert.False(exists);
        }
    };
    public class checkENV
    {
        Uploader up = new Uploader();

        [Fact]
        public void checkFolderID()
        {
            string? _FolderID = up._FolderId;
            Assert.NotNull(_FolderID);
        }
        [Fact]
        public void checkFolderId()
        {
            string? FolderId = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_FOLDER_ID");
            Assert.NotNull(FolderId);
        }
        [Fact]
        public void checkPrivateKEY()
        {
            string? _PrivateKEY = up._PrivateKey;
            Assert.NotNull(_PrivateKEY);
        }
        [Fact]
        public void checkPrivateKey()
        {
            string? PrivateKey = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_PRIVATE_KEY");
            Assert.NotNull(PrivateKey);
        }
        [Fact]
        public void checkClientEMAIL()
        {
            string? _ClientEMAIL = up._ClientEmail;
            Assert.NotNull(_ClientEMAIL);
        }
        [Fact]
        public void checkClientEmail()
        {
            string? ClientEmail = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_CLIENT_EMAIL");
            Assert.NotNull(ClientEmail);
        }
        [Fact]
        public void checkServiceAccountTYPE()
        {
            string? _ServiceAccountTYPE = up._ServiceAccountType;
            Assert.NotNull(_ServiceAccountTYPE);
        }
        [Fact]
        public void checkServiceAccountType()
        {
            string? ProjectType = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_PROJECT_TYPE");
            Assert.NotNull(ProjectType);
        }
    };

    public class UploaderTest
    {
        public string? _downlodPath = Environment.GetEnvironmentVariable("DOWNLOAD");

        [Fact]
        public void checkUploadDownloadFiles()
        {
            Uploader upl = new Uploader();
            var _filePath = Path.GetTempFileName();
            var _fileName = Path.GetFileName(_filePath);
            var UploadedFileId = upl.UploadFile(_filePath);
            var _DownloadPath = $"{_downlodPath}{_fileName}";
            var DownloadedFileId = upl.DownloadFile(UploadedFileId, _DownloadPath);
            Assert.Equal(UploadedFileId, DownloadedFileId);
            FileInfo UploadedFile = new FileInfo(_filePath);
            var UploadedFileSize = UploadedFile.Length;
            FileInfo DownloadedFile = new FileInfo(_DownloadPath);
            var DownloadedFileSize = DownloadedFile.Length;
            Assert.Equal(UploadedFileSize, DownloadedFileSize);
        }
    };
}
