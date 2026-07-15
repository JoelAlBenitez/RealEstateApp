using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto;
using RealEstateApp.Core.Application.Contracts.FileManager;
using RealEstateApp.Core.Application.DTOs.FileManager;

namespace RealEstateApp.Infraestructure.Shared.Services.FileManager
{
    public sealed class FileManager : IFileManager
    {


        public Task<bool> DeleteAsync(string folderName, string Id)
        {
            string basePath = $"Img/{folderName}/{Id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{basePath}");
            if (Directory.Exists(path))
            {

                Directory.Delete(path, true);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
        

        public Task<bool> DeleteManyAsync(IEnumerable<string> Ids, string folderName)
        {
            bool deletedAny = false;

            foreach (var id in Ids)
            {
                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "Img",
                    folderName,
                    id);

                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    deletedAny = true;
                }
            }

            return Task.FromResult(deletedAny);

        }

        public async Task<string> SaveAsync(IFormFile fileManager, string folderName, string Id)
        {
            string basePath = $"Img/{folderName}/{Id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/{basePath}");

            if (fileManager == null || fileManager.Length == 0) return string.Empty;
            if (fileManager.Length > 5 * 1024 * 1024) return string.Empty;

            //extension permitidas
            var extension = Path.GetExtension(fileManager.FileName).ToLowerInvariant();
            var allowExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var allowdTypes = new[] { "image/jpg", "image/jpeg", "image/png", "image/webp" };
            if (!allowExtensions.Contains(extension) || !allowdTypes.Contains(fileManager.ContentType)) return string.Empty;

            if (!await HasValidImageSignatureAsync(fileManager, extension)) return string.Empty;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);


            //ruta completa creada y lectura de la imagen pertinente

            FileInfo fileInfo = new(fileManager.FileName);
            string fileName = Id + fileInfo.Extension;
            var fullFilePath = Path.Combine(path, fileName);
            await using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await fileManager.CopyToAsync(stream);

            }
           
            return $"{basePath}/{fileName}";
        }

        private static async Task<bool> HasValidImageSignatureAsync(IFormFile fileManager, string extension)
        {
            byte[] header = new byte[12];
            await using var stream = fileManager.OpenReadStream();
            int read = await stream.ReadAtLeastAsync(header, header.Length, throwOnEndOfStream: false);

            return extension switch
            {
                ".jpg" or ".jpeg" => read >= 3 && header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF,
                ".png" => read >= 8 && header[0] == 0x89 && header[1] == 0x50 && header[2] == 0x4E && header[3] == 0x47
                                    && header[4] == 0x0D && header[5] == 0x0A && header[6] == 0x1A && header[7] == 0x0A,
                ".webp" => read >= 12 && header[0] == (byte)'R' && header[1] == (byte)'I' && header[2] == (byte)'F' && header[3] == (byte)'F'
                                      && header[8] == (byte)'W' && header[9] == (byte)'E' && header[10] == (byte)'B' && header[11] == (byte)'P',
                _ => false
            };
        }

        public async Task<FileManagersMultipleFiles> SaveManyAsync(IEnumerable<IFormFile> files, string folderName)
        {
            List<string> urls = [];
            List<string> ids = [];
            int count = 0;
            foreach (var file in files)
            {
                string id = Guid.NewGuid().ToString();
                string url = await SaveAsync(file, folderName, id);
     
                if (string.IsNullOrWhiteSpace(url))
                {
                    count++;
                    // Eliminar las imágenes que ya se habían guardado
                   await DeleteManyAsync(ids, folderName);

                    return new FileManagersMultipleFiles
                    {
                        Files = urls,
                        NumberFailed = count
                    };
                }

                ids.Add(id);
                urls.Add(url);
            }

            return new FileManagersMultipleFiles
            {
                Files = urls,
                NumberFailed = count
            };
        }
    }
}
