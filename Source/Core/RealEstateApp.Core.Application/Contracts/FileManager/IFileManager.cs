namespace RealEstateApp.Core.Application.Contracts.FileManager
{
    public interface IFileManager
    {
        Task<string?> SaveAsync(IFileManager fileManager, string folderName, string Id);
        Task<bool> DeleteAsync(string folderName, string Id);
    }
}
