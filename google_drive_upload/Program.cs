using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using System.CommandLine;
using System.Text.RegularExpressions;
using System.CommandLine.NamingConventionBinder;
using System.Reflection;
using Google.Apis.Download;
namespace google_drive_upload
{
    public class Uploader
    {
        public string? _folderId = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_FOLDER_ID");
        public string? _ClientId = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_CLIENT_ID");
        public string? _ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_UPLOAD_SECRET_KEY");
        public string? _temp = Environment.GetEnvironmentVariable("TEMP");
        public string? _downlodPath = Environment.GetEnvironmentVariable("DOWNLOAD");
        public string? _PathToCredentials = Environment.GetEnvironmentVariable("CREDENTIALS");
        public const string _regex = @"^[\w\-. ]+$";
        public int Check_Preconditions(string _FileName)
        {
            if (!(Regex.IsMatch(_FileName, _regex)))
            {
                Console.WriteLine("Non valid file name! Please rename your file using 'A-Z', 'a-z' letters, '0-9' numbers, '-' and '_'.");
                return -1;
            }
            if (String.IsNullOrEmpty(_ClientId))
            {
                Console.WriteLine("GOOGLE_UPLOAD_CLIENT_ID - environment variable not found! Please check the right value of GOOGLE_UPLOAD_CLIENT_ID");
                return -2;
            }
            if (String.IsNullOrEmpty(_ClientSecret))
            {
                Console.WriteLine("GOOGLE_UPLOAD_SECRET_KEY - environment variable not found!\n Please check the right value of GOOGLE_UPLOAD_SECRET_KEY");
                return -3;
            }
            if (String.IsNullOrEmpty(_folderId))
            {
                Console.WriteLine("GOOGLE_UPLOAD_FOLDER_ID - environment variable not found!\n Please check the right value of GOOGLE_UPLOAD_FOLDER_ID");
                return -4;
            }
            return 0;
        }
        public bool FileExists(string _filePath)
        {
            if (File.Exists(_filePath))
            {
                return true;
            }
            Console.WriteLine("File does not exists! Please check the filePath.");
            return false;
        }
        protected DriveService GetService()
        {
            var credentials = GoogleCredential.FromFile(_PathToCredentials).CreateScoped(DriveService.ScopeConstants.Drive);

            var service = new Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials
            });
            return service;

        }
        public string UploadFile(string __fileName, string __filePath)
        {
            DriveService service = GetService();
            var fileMetadata = new Google.Apis.Drive.v3.Data.File();
            fileMetadata.Name = __fileName;
            if (_folderId is not null)
                fileMetadata.Parents = new List<string> { _folderId };
            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(__filePath, FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, String.Empty);
                Google.Apis.Upload.IUploadProgress? response = null;
                try
                {
                    response = request.Upload();
                }
                catch
                {
                    if (response is not null)
                        Console.WriteLine($"Error uploading file: {response.Exception.Message}");
                }
                Console.WriteLine("File uploaded!");
                var file = request.ResponseBody;
                return file.Id;
            }
        }
        public string DownloadFile(string fileId, string filePath)
        {
            DriveService service = GetService();
            var request = service.Files.Get(fileId);
            using (var memoryStream = new MemoryStream())
            {
                request.MediaDownloader.ProgressChanged += (IDownloadProgress progress) =>
                {
                    switch (progress.Status)
                    {
                        case Google.Apis.Download.DownloadStatus.Completed:
                            {
                                Console.WriteLine("Download Complete!");
                                break;
                            }
                        case Google.Apis.Download.DownloadStatus.Failed:
                            {
                                Console.WriteLine("Download Failed!");
                                break;
                            }
                    }
                };
                request.Download(memoryStream);
                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(memoryStream.GetBuffer(), 0, memoryStream.GetBuffer().Length);
                }
            }
            return fileId;
        }
    }
    public static class Program
    {
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand
            {
                new Argument<string>("filename", "File to be uploaded!"),
                new Option(
                "--fp",
                description: "Filepath is missing. Help!")
            }.WithHandler(nameof(CMDHelperAsync));
            rootCommand.Description = "Utility ver 1.0 which uploads a file on specified google driver. \nPlease provide ENV variables:  GOOGLE_UPLOAD_FOLDER_ID, GOOGLE_UPLOAD_CLIENT_ID,\nGOOGLE_UPLOAD_SECRET_KEY. Args[0] is which is to be uploaded to Google Drive.\nPlease check the right file PATH...Missing arguments!";
            return rootCommand.Invoke(args);
        }
        private static int CMDHelperAsync(Google.Apis.Drive.v3.Data.File file, string filename)
        {
            Uploader up = new Uploader();
            Console.WriteLine($"File to be uploaded {filename}");
            string _filePath = Path.GetFullPath(filename);
            string _fileName = Path.GetFileName(_filePath);
            string downloadPath = $"{up._downlodPath}{_fileName}";
            up.FileExists(_filePath);
            int err = up.Check_Preconditions(_fileName);
            if (err < 0)
                return err;
            var fileId = up.UploadFile(_fileName, _filePath);
            up.DownloadFile(fileId, downloadPath);
            return 0;
        }
        private static Command WithHandler(this Command command, string methodName)
        {
            var method = typeof(Program).GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static);
            var handler = CommandHandler.Create(method!);
            command.Handler = handler;
            return command;
        }
    }
}