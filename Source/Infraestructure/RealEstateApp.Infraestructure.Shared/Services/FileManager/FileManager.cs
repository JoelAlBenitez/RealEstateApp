using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Contracts.FileManager;

namespace RealEstateApp.Infraestructure.Shared.Services.FileManager
{
    public sealed class FileManager : IFileManager
    {
        //por impletar metodos de almacenamientos de imagenes en el wwwroot / agregar
        //metodo de extraccion de Ids pertinentes y folder para facilitar la eliminacion de las imagenes al
        //momentpo de fallos de creacion del registro en la bd o eliminaciones por solicitud del usuario

        public Task<bool> DeleteAsync(string folderName, string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteManyAsync(IEnumerable<string> Ids, string folderName)
        {
            throw new NotImplementedException();
        }

        public Task<string?> SaveAsync(IFormFile fileManager, string folderName, string Id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SaveManyAsync(IEnumerable<IFormFile> files, string folderName)
        {
            throw new NotImplementedException();
        }
    }
}
