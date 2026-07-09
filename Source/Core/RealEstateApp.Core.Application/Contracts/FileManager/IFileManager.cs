using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Core.Application.Contracts.FileManager
{
    public interface IFileManager
    {
        Task<string?> SaveAsync(IFormFile fileManager, string folderName, string Id);
        Task<bool> DeleteAsync(string folderName, string Id);
        Task<IReadOnlyList<string>> SaveManyAsync(IEnumerable<IFormFile> files, string folderName);
        Task<bool> DeleteManyAsync(IEnumerable<string> Ids, string folderName);
    }
}
