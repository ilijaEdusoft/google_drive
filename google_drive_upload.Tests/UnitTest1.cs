using google_drive_upload;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace google_drive_upload.Tests
{

    public class FileNameTest
    {
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
        //vo testot za file dali postoi dodadi i so preconditions dali raboti

        //vo configure na testot, treba da gi setirahs ENV variajblite i odsetirash za da proverish logikata
        public void checkFileName(string input, bool expected)
        {
            string? reg = Program._regex;
            var r = new Regex(reg, RegexOptions.IgnoreCase);
            Assert.Equal(expected, r.Match(input).Success);
        }

        [Fact]
        public void IsFileExists()
        {
            var tempFilePath = Path.GetTempFileName();
            var file = new FileInfo(tempFilePath);
            Assert.True(file.Exists);
            file.Delete();
        }

    };

    public class checkENV
    {
        [Fact]
        public void checkClientID()
        {
            string? _clientID = Program._ClientId;
            Assert.NotNull(_clientID);
        }
        [Fact]
        public void checkClientSecret()
        {
            string? _clientSecret = Program._ClientSecret;
            Assert.NotNull(_clientSecret);
        }
        [Fact]
        public void checkFolderID()
        {
            string? _folderID = Program._folderId;
            Assert.NotNull(_folderID);
        }
    };
}
