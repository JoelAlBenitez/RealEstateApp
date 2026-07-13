using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using RealEstateApp.Core.Application.Contracts.Users.Validation;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
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
        public const string EmailRegex =
            @"^(?=.{1,254}$)(?=.{1,64}@)[A-Za-z0-9]+(?:[._%+-][A-Za-z0-9]+)*@[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*(?:\.[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*)+$";
        private readonly IUserSession _userSession;

        public ServicesValidateUsers(
            UserManager<AppUsers> userManager,
            IUserSession userSession
            )
        {
            _userManager = userManager;
            _userSession = userSession;
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
            if (exitsEmail != null)
            {
                response.HasError = true;
                response.Errors.Add("El email ingresado ya se encuentra asociado, favor prueve con otro correo");
                return response;
            }
            if (exitsEmail!.UserName == externalUsersDto.NameUser)
            {
                response.HasError = true;
                response.Errors.Add("El nombre de usuario ingresado ya se encuentra ocupado, favor intente de nuevo mas tarde");
                return response;
            }


            #endregion

            return response;

        }
        public async Task<EditResponseDto> UpdateExternalValidateUserAsync
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
            if (existUser == null)
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


        #region internal users methods

        public async Task<UserResponseDto> CreateInternalValidateUserAsync
            (RegisterInternalUsersDto internalUsersDto,
            UserResponseDto response)
        {
            if (internalUsersDto.TypeUser != (int)Roles.Desarrollador
                && internalUsersDto.TypeUser != (int)Roles.Administrador)
            {
                response.HasError = true;
                response.Errors.Add("El rol especficado para este usuario no es valido");
                return response;
            }

            #region validate fields
            if (string.IsNullOrWhiteSpace(internalUsersDto.Email) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.IDCard) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.NameUser) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.Name) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.LastName) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.Password) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.ConfirmPassword)

                 )
            {

                response.HasError = true;
                response.Errors.Add("Debe rellenar todos los campos del usuario");
                return response;
            }
            if (internalUsersDto.Password != internalUsersDto.ConfirmPassword)
                response.Errors.Add("Las contraseñas deben coincidir");
            if (!Regex.IsMatch(internalUsersDto.IDCard, @"^\d{11}$"))
                response.Errors.Add("La cedula ingresada no contiene un formato valido.");
            if (response.Errors.Any())
            {
                response.HasError = true;
                return response;
            }


            #endregion


            #region validate existencias
            var existUserName = await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.UserName == internalUsersDto.NameUser);
            if (existUserName)
            {
                response.HasError = true;
                response.Errors.Add("El nombre de usuario ingresado no se encuentra disponible.");
                return response;
            }

            var existEmail = await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == internalUsersDto.Email);
            if (existEmail) {

                response.HasError = true;
                response.Errors.Add("El correo electronico ingresado ya se encuentra asociado a una cuenta, favor ingrese uno diferente.");
                return response;
            }
            var existIdCard = await _userManager.Users.AsNoTracking()
                .AnyAsync(u => u.IDCard == internalUsersDto.IDCard);
            if (existIdCard)
            {
                response.HasError = true;
                response.Errors.Add("La cedula ingresada ya se encuentra asociada a una cuenta.");
                return response;
            }

            #endregion


            return response;
        }


        public async Task<EditResponseDto> UpdateInternalValidateUserAsync
            (EditInternalUserDto editInternalUserDto, EditResponseDto response)
        {

            if (editInternalUserDto.Id == _userSession.GetIdCurrentUser())
            {
                response.HasError = true;
                response.Errors.Add("No puede editar su propio usuario desde este mantenimiento");
                return response;
            }
            if (string.IsNullOrWhiteSpace(editInternalUserDto.Email)
                || string.IsNullOrWhiteSpace(editInternalUserDto.UserName)
                || string.IsNullOrWhiteSpace(editInternalUserDto.Name)
                || string.IsNullOrWhiteSpace(editInternalUserDto.LastName)
                || string.IsNullOrWhiteSpace(editInternalUserDto.IdCard)
                )
            {
                response.HasError = true;
                response.Errors.Add("Debe completar el llenado de los diversos datos de forma correcta");
                return response;
            }

            var existUser = await _userManager.FindByIdAsync(editInternalUserDto.Id);
            if(existUser == null)
            {
                response.HasError = true;
                response.Errors.Add("El usuario especificado no fue encontrado, favor intente de nuevo.");
                return response;
            }
            var roles = await _userManager.GetRolesAsync(existUser);
            if(!roles.Contains(Roles.Desarrollador.ToString()) &&
                !roles.Contains(Roles.Administrador.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("El usuario especificado no puede ser modificado desde este mantenimiento.");
                return response;
            }
            var existOtherUserByEmail = await _userManager.Users.AsNoTracking()
                .AnyAsync(u => u.Email == editInternalUserDto.Email && u.Id != editInternalUserDto.Id);
            var existOtherUsersByUserName = await _userManager.Users.AsNoTracking()
                .AnyAsync(u => u.UserName == editInternalUserDto.UserName && u.Id != editInternalUserDto.Id);
            var existOtherUsersByIdCard = await _userManager.Users.AsNoTracking()
                .AnyAsync(u => u.IDCard == editInternalUserDto.IdCard && u.Id != editInternalUserDto.Id);
            if (existOtherUserByEmail)
                response.Errors.Add("El correo ingresado ya se encuentra en uso.");
            if (editInternalUserDto.NewPassword != editInternalUserDto.ConfirmNewPassword)
                response.Errors.Add("Las contraseñas deben coincidir.");
            if (existOtherUsersByUserName)
                response.Errors.Add("El nombre de usuario ingresado no se encuentra disponible.");
            if (existOtherUsersByIdCard)
                response.Errors.Add("La cedula ingresada ya se encuentra asociada a una cuenta.");
            if (response.Errors.Any())
            {
                response.HasError = true;
                return response;
            }
           


            return response;
        }

        #endregion

    }
}
