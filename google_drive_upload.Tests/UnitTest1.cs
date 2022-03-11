using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;
using google_drive_upload;
using Google.Apis.Drive.v3;
using System.Collections.Generic;
using Google.Apis.Services;
using Google.Apis.Auth.OAuth2;
using System.Threading;

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
        public void preconditionsCheckF()
        {
            var tempFile = Path.GetTempFileName();
            var error = up.Check_Preconditions(tempFile);
            Assert.NotNull(error);
        }
        [Fact]
        public void preconditionsCheckNF()
        {
            var tempFile = Path.GetTempFileName();
            var _file = new FileInfo(tempFile);
            _file.Delete();
            if ((String.IsNullOrEmpty(tempFile)))
            {
                var error1 = up.Check_Preconditions(tempFile);
                Assert.Null(error1);
            }
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
        public void checkUploader()
        {
            Uploader up = new Uploader();

            var tmpFile = Path.GetTempPath();
            var tmpName = Path.GetTempFileName();
            var uploadedFile = up.UploadFile(tmpFile, tmpName);
            Assert.NotNull(uploadedFile);
            Assert.NotEmpty(uploadedFile);
        }
    };

}

