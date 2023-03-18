using Microsoft.EntityFrameworkCore;
using OrganizationEmployeeAPI.Contracts;
using OrganizationEmployeeAPI.Models;

namespace OrganizationEmployeeAPI.Repository
{
    public class FileRepository : IFileService
    {
        private readonly APIDbContext _appDBContext;
        private readonly IWebHostEnvironment _env;

        public FileRepository(APIDbContext context, IWebHostEnvironment environment)
        {
            this._appDBContext = context ?? throw new ArgumentNullException(nameof(context));
            this._env = environment;
        }

        public async Task PostFileAsync(IFormFile fileData, FileType fileType)
        {
            if (string.IsNullOrWhiteSpace(_env.WebRootPath))
            {
                _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }

            var uploads = Path.Combine(_env.WebRootPath, "Photos");
            var filePath = Path.Combine(uploads, fileData.FileName);

            if(!Directory.Exists(uploads))
                _ = Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            try
            {
                var fileDetails = new FileDetails()
                {
                    ID = 0,
                    FileName = fileData.FileName,
                    FileType = fileType,
                };

                //using (var stream = new MemoryStream())
                //{
                //    fileData.CopyTo(stream);
                //    fileDetails.FileData = stream.ToArray();
                //}

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    fileData.CopyTo(stream);
                    byte[] bytes = new byte[stream.Length];
                    fileDetails.FileData = bytes.ToArray(); //save the file data to database
                }

                var result = _appDBContext.FileDetails.Add(fileDetails);
                await _appDBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task PostMultiFileAsync(List<FileUploadModel> fileData)
        {
            try
            {
                foreach (FileUploadModel file in fileData)
                {
                    var fileDetails = new FileDetails()
                    {
                        ID = 0,
                        FileName = file.FileDetails.FileName,
                        FileType = file.FileType,
                    };

                    using (var stream = new MemoryStream())
                    {
                        file.FileDetails.CopyTo(stream);
                        fileDetails.FileData = stream.ToArray();
                    }

                    var result = _appDBContext.FileDetails.Add(fileDetails);
                }
                await _appDBContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DownloadFileById(int Id)
        {
            try
            {
                var file = _appDBContext.FileDetails.Where(x => x.ID == Id).FirstOrDefaultAsync();

                var content = new MemoryStream(file.Result.FileData);
                var path = Path.Combine(
                   Directory.GetCurrentDirectory(), "FileDownloaded",
                   file.Result.FileName);

                await CopyStream(content, path);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CopyStream(Stream stream, string downloadPath)
        {
            using var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);
        }
    }
}
