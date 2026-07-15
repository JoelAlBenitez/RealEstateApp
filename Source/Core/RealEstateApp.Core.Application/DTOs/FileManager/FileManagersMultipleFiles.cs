namespace RealEstateApp.Core.Application.DTOs.FileManager
{
    public sealed class FileManagersMultipleFiles
    {
        public required List<string> Files { get; set; }
        public required int NumberFailed { get; set; }
    }
}
