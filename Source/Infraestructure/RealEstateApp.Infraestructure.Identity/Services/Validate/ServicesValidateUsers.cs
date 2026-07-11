using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Contracts.Users.Validation;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;
using System.Text.RegularExpressions;
namespace RealEstateApp.Infraestructure.Identity.Services.Validate
{
    public sealed class ServicesValidateUsers
        : IServicesValidateUsers
    {

        private readonly UserManager<AppUsers> _userManager;
        public const string EmailRegex = @"^(?=.{1,254}$)(?=.{1,64}@)[A-Za-z0-9]+(?:[._%+-][A-Za-z0-9]+)*@[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*(?:\.[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*)+$";
        public ServicesValidateUsers(UserManager<AppUsers> userManager)
        {
            _userManager = userManager;
        }

        #region methods externals users
        public async Task<UserResponseDto> CreateExternalValidateUserAsync
            (RegisterExternalUsersDto externalUsersDto,
            UserResponseDto response
            )
        {

            #region validate fields
            if (string.IsNullOrWhiteSpace(externalUsersDto.Email) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.PhoneNumber) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.NameUser) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.Name) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.LastName) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.Password) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.ConfirmPassword)

                 )
            {

                response.HasError = true;
                response.Errors.Add("Debe rellenar todos los campos del usuario");
                return response;
            }

            if (string.IsNullOrWhiteSpace(externalUsersDto.ProfileImg))
                response.Errors.Add("La imagen ingresa no pudo se procesada verifique" +
                    " si la misma tiene un formato valido (JPG, PNG, JPEG) y no mauor a 5 mb ");

            if (externalUsersDto.TypeUser != (int)Roles.Agente
                || externalUsersDto.TypeUser != (int)Roles.Cliente)
                response.Errors.Add("El tipo de usuario especificado no es valido");

            if (!Regex.IsMatch(externalUsersDto.PhoneNumber, "^(809|829|849)-\\d{3}-\\d{4}$"))
                response.Errors.Add("Debe ingresar un número telefónico válido de República Dominicana.");
            if (!Regex.IsMatch(externalUsersDto.Email, EmailRegex))
                response.Errors.Add("Debe ingresar un correo eletronico valido");
            if (externalUsersDto.Password != externalUsersDto.ConfirmPassword)
                response.Errors.Add("Las contraseñas deben coincidir");

            if (response.Errors.Any())
            {
                response.HasError = true;
                return response;
            }
            #endregion


            #region validate exits user

            var exitsEmail = await _userManager.FindByEmailAsync(externalUsersDto.Email);
            if(exitsEmail != null)
            {
                response.HasError = true;
                response.Errors.Add("El email ingresado ya se encuentra asociado, favor prueve con otro correo");
                return response;
            }
            if(exitsEmail!.UserName == externalUsersDto.NameUser)
            {
                response.HasError = true;
                response.Errors.Add("El nombre de usuario ingresado ya se encuentra ocupado, favor intente de nuevo mas tarde");
                return response;
            }


            #endregion

            return response;

        }
        public  async Task<EditResponseDto> UpdateExternalValidateUserAsync
           (EditAgentUserDto editAgentUserDto, EditResponseDto response)
        {

            #region validate fields
            if (string.IsNullOrWhiteSpace(editAgentUserDto.Name) ||
               string.IsNullOrWhiteSpace(editAgentUserDto.LastName) ||
            string.IsNullOrWhiteSpace(editAgentUserDto.PhoneNumber) ||
            string.IsNullOrWhiteSpace(editAgentUserDto.ProfileImg))
            {
                response.HasError = true;
                response.Errors.Add("Datos invalidos, favor complete los datos correctamente");
                return response;
            }
            if (!Regex.IsMatch(editAgentUserDto.PhoneNumber, "^(809|829|849)-\\d{3}-\\d{4}$"))
                response.Errors.Add("Debe ingresar un número telefónico válido de República Dominicana.");

            if (editAgentUserDto.ChangePorfileImg && string.IsNullOrWhiteSpace(editAgentUserDto.ProfileImg))
                response.Errors.Add("La imagen ingresa no pudo se procesada verifique" +
                        " si la misma tiene un formato valido (JPG, PNG, JPEG) y no mauor a 5 mb ");
            if (response.Errors.Any())
            {
                response.HasError = true;
                return response;
            }
            #endregion

            #region validate user
            var existUser = await _userManager.FindByIdAsync(editAgentUserDto.Id);
            if(existUser == null)
            {
                response.HasError = true;
                response.Errors.Add("Oops, Al parecer su usuario presenta problemas favor, intente de nuevo mas tarde.");
                return response;
            }
            var users = await _userManager.GetRolesAsync(existUser);
            if (!users.Contains(Roles.Agente.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("No posee los privilegios suficientes para ejecutar esta operacion");
                return response;
            }

            #endregion

            return response;

        }


        #endregion

        public Task<UserResponseDto> CreateInternalValidateUserAsync
            (RegisterInternalUsersDto internalUsersDto, UserResponseDto response)
        {
            throw new NotImplementedException(); //implementar en la rama de api
        }

     
        public Task<EditResponseDto> UpdateInternalValidateUserAsync
            (EditInternalUserDto editInternalUserDto, UserResponseDto response)
        {
            throw new NotImplementedException(); //implementar en la rama de la api
        }

       
    }
}
