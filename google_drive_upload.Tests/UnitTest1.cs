using System;
using System.IO;
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
        public void checkClientID()
        {
            string? _clientID = up._ClientId;
            Assert.NotNull(_clientID);
        }
        [Fact]
        public void checkClientId()
        {
            string? clienId = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_CLIENT_ID");
            Assert.NotNull(clienId);
        }
        [Fact]
        public void checkClientSECRET()
        {
            string? _clientSecret = up._ClientSecret;
            Assert.NotNull(_clientSecret);
        }
        [Fact]
        public void checkClientSecret()
        {
            string? clientSecret = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_SECRET_KEY");
            Assert.NotNull(clientSecret);
        }
        [Fact]
        public void checkFolderID()
        {
            string? _folderID = up._folderId;
            Assert.NotNull(_folderID);
        }
        [Fact]
        public void checkFolderId()
        {
            string? folderId = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_FOLDER_ID");
            Assert.NotNull(folderId);
        }
    };

    public class UploaderTest
    {
        [Fact]
        public void checkUploadDownloadFiles()
        {
            Uploader upl = new Uploader();
            var filePath = Path.GetTempFileName();
            var fileName = Path.GetFileName(filePath);
            var _uploader = upl.UploadFile(fileName, filePath);
            string? downloadPath = $"{upl._temp}{fileName}";
            var _downloader = upl.DownloadFile(_uploader, downloadPath);
            if (!String.IsNullOrEmpty(downloadPath))
                Assert.Equal(_uploader, _downloader);
            var uploader = _uploader.Length;
            var downloader = _downloader.Length;
            Assert.Equal(uploader, downloader);
        }
    };
}