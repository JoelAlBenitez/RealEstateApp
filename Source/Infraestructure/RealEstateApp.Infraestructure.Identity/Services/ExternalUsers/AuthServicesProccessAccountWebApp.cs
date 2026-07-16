using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using RealEstateApp.Core.Application.Contracts.EmailServices;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Message;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Interfaces;
using System.Text;

namespace RealEstateApp.Infraestructure.Identity.Services.ExternalUsers
{
    public sealed class AuthServicesProcccessAccountWebApp : IAuthProcesssAccountWebApp
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly IGenerateTokens _generateTokens;
        private readonly IEmailService _emailServices;
            
        public AuthServicesProcccessAccountWebApp(UserManager<AppUsers
            > userManager, SignInManager<AppUsers> signInManager,
            IGenerateTokens generateTokens,
            IEmailService emailService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _generateTokens = generateTokens;
            _emailServices = emailService;
        }

        #region process account
        public async Task<string> ConfirmAccountByEmailAsync(string token, string userId)
        {
            var existUser = await _userManager.FindByIdAsync( userId );
            if (existUser == null) {
                return "No fue posible verificar su cuenta. Intente nuevamente más tarde.";
            }
            var roles = await _userManager.GetRolesAsync(existUser);
            if (roles.Contains(Roles.Cliente.ToString()))
            {
                var tokerVery = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
                var result = await _userManager.ConfirmEmailAsync(existUser, tokerVery);
                if (!result.Succeeded)
                {
                    return "Ocurrió un error al verificar su cuenta. Intente nuevamente más tarde.";
                }

                existUser.BlockedEmailSending = null;
                existUser.IsActive = true;
                await _userManager.UpdateSecurityStampAsync(existUser);
                return "Su cuenta fue confirmada correctamente. Ya puede iniciar sesión y disfrutar de RealEstateApp.";
            }

            return "Su cuenta no puede ser activada desde este apartado. Favor de contactar con un administrador.";
        }

        public async Task<UserResponseDto> ForgoutPasswordAsync(
            ForgoutPasswordDto forgoutPasswordDto)
        {
            var response = new UserResponseDto
            {
                Errors = new List<string>(),
                HasError = false,
                Roles = null!,
            };
            var existUser = await _userManager.FindByNameAsync(forgoutPasswordDto.UserName);
            var validate = ValidateFields(response, forgoutPasswordDto, existUser!);
            if (validate != null && validate.HasError) return validate;

            if (existUser!.EmailConfirmed && existUser.IsActive)
            {
                var generateTokenResetPassword = await _generateTokens.GenerateTokenResetPassword(existUser, forgoutPasswordDto.Origin);
                if (string.IsNullOrWhiteSpace(generateTokenResetPassword))
                {
                    response.HasError = true;
                    response.Errors.Add("La solicitud no pudo ser procesada en este momento. Intente nuevamente más tarde.");
                    return response;
                }
                existUser.BlockedEmailSending = DateTimeOffset.UtcNow.AddMinutes(5);
                var update = await _userManager.UpdateAsync(existUser);
                if (!update.Succeeded)
                {
                    response.HasError = true;
                    response.Errors.Add("La solicitud no pudo ser procesada en este momento. Intente nuevamente más tarde.");
                    return response;
                }
                {
                    var sendEmail = await _emailServices.SendEmailAsync(new MessageDto
                    {
                        To = existUser.Email!,
                        Subject = "RealEstateApp",
                        Body = "<div style = 'background:#4f46e5;color:white;padding:20px;" +
                                    "text-align:center;font-size:20px;' >" +
                                               "<h2> RealEstateApp </h2> " +
                                               "<p> Ha solicitado un cambio de contraseña para su cuenta </p>" +
                                               $"<p style = 'color:#fff;' >{generateTokenResetPassword}<p>" +
                                               "<p><b>Nota:</b> Si usted no ha realizado esta solicitud ignore este mensaje.</p>" +
                                               " </div>"

                    });
                    if (!sendEmail)
                    {
                        response.HasError = true;
                        response.Errors.Add("Este servicio no se encuentra disponible en este momento. Intente nuevamente más tarde.");
                        return response;
                    }


                }
            }
            return response;
        }

        public async Task<UserResponseDto> LoginUser(LoginDto loginDto)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Errors = new List<string>(),
                Roles = new List<string>()
            };

            var existUser = await _userManager.FindByNameAsync(loginDto.EmailOrNameUser);
            if (existUser != null)
            {
                var validate = await ValidateLogin(response, existUser, loginDto);
                if (validate != null && validate.HasError) return validate;
                var roles = await _userManager.GetRolesAsync(existUser);
                response.Roles = roles.ToList();
                return response;
            }
            var existUserByEmail = await _userManager.FindByEmailAsync(loginDto.EmailOrNameUser);
            if (existUserByEmail != null)
            {
                var validate = await ValidateLogin(response, existUserByEmail, loginDto);
                if (validate != null && validate.HasError) return validate;
                var roles = await _userManager.GetRolesAsync(existUserByEmail);
                response.Roles = roles.ToList();

                return response;
            }
            response.HasError = true;
            response.Errors.Add("Los datos de acceso son inválidos.");
            return response;


        }

        public async Task<UserResponseDto> ResendActivationEmailAsync(
            ResendActivationEmailDto resendActivationEmailDto)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Roles = null!,
                Errors = new List<string>(),
            };
            var existUser = await _userManager.FindByNameAsync(resendActivationEmailDto.UserName);
            var validate = await ValidateResentEmailConfirm(response, existUser!, resendActivationEmailDto);
            if (validate != null && validate.HasError) return validate;
           
                await _userManager.UpdateSecurityStampAsync(existUser!);
                var generateTokens = await _generateTokens.GenerateTokenConfirmEmail(existUser!, resendActivationEmailDto.Origin);

                if (string.IsNullOrWhiteSpace(generateTokens))
                {
                    response.HasError = true;
                    response.Errors.Add("Esta función no se encuentra disponible en este momento. Intente nuevamente más tarde.");
                    return response;
                }

                existUser!.EmailConfirmed = false;
                existUser.IsActive = false;
                var update = await _userManager.UpdateAsync(existUser);

                if (update.Succeeded)
                {
                    existUser.BlockedEmailSending = DateTimeOffset.UtcNow.AddMinutes(5);
                    var send = await _emailServices.SendEmailAsync(new MessageDto
                    {
                        To = existUser.Email!,
                        Subject = "RealEstateApp",
                        Body = "<div style = 'background:#4f46e5;color:white;padding:20px;text-align:center;font-size:20px;' >" +
                            "<h2> RealEstateApp </h2> " +
                            "<p> Ha solicitado un email de confirmación para su cuenta </p>" +
                            $"<p style = 'color:#fff;' >{generateTokens}<p>" +
                            "<p><b>Nota:</b> Si usted no ha realizado esta solicitud ignore este mensaje.</p>" +
                            " </div>",
                    });
                    if (!send)
                    {
                        response.HasError = true;
                        response.Errors.Add("Esta función no se encuentra disponible en este momento. Intente nuevamente más tarde.");
                        return response;
                    }

                    await _userManager.UpdateAsync(existUser);
                    return response;
                }

                response.HasError = true;
                response.Errors.Add("Esta función no se encuentra disponible en este momento. Intente nuevamente más tarde.");
                return response;
          
       }

        public async Task<UserResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Roles = null!,
                Errors = new List<string>(),
            };

            var exitsUser = await  _userManager.FindByIdAsync(resetPasswordDto.Id);
            if (exitsUser == null)
            {

                response.HasError = true;
                response.Errors.Add("Este usuario no se encuentra asociado a ninguna cuenta. Regístrese y disfrute de RealEstateApp.");
                return response;
            }
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
            {
                response.HasError = true;
                response.Errors.Add("La contraseña y la confirmación de contraseña no coinciden.");
                return response;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDto.Token));
            var verifyResetPassword = await _userManager.ResetPasswordAsync(exitsUser, token, resetPasswordDto.NewPassword);
            if (!verifyResetPassword.Succeeded)
            {
                response.HasError = true;
                response.Errors.AddRange(verifyResetPassword.Errors.Select(e => e.Description).ToList());
                return response;
            }

            exitsUser.BlockedEmailSending = null;
            exitsUser.AccessFailedCount = 0;

            await _userManager.UpdateSecurityStampAsync(exitsUser);
            await _userManager.ResetAccessFailedCountAsync(exitsUser);
            await _userManager.SetLockoutEndDateAsync(exitsUser, null);
            return response;
        }
        #endregion

        #region methods validations 

        private UserResponseDto ValidateFields(UserResponseDto response, 
            ForgoutPasswordDto forgoutPasswordDto, AppUsers existUser)
        {
            #region validaciones
            if (string.IsNullOrWhiteSpace(forgoutPasswordDto.Origin))
            {
                response.HasError = true;
                response.Errors.Add("Esta funcionalidad no se encuentra disponible en este momento. Intente nuevamente más tarde.");
                return response;
            }
            if (existUser == null)
            {
                response.HasError = true;
                response.Errors.Add("Debe ingresar un nombre de usuario válido.");
                return response;
            }

            if (existUser.BlockedEmailSending.HasValue &&
                existUser.BlockedEmailSending!.Value > DateTimeOffset.UtcNow
                )
            {
                response.HasError = true;
                response.Errors.Add("Ya solicitó un correo de confirmación o de cambio de contraseña recientemente. Debe esperar 5 minutos antes de solicitar otro.");
                return response;
            }
            #endregion

            return response;
        }

        private async Task<UserResponseDto> ValidateLogin(UserResponseDto response,
            AppUsers user, LoginDto dto)
        {
            var rolesUser = await _userManager.GetRolesAsync(user);

            if (string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.EmailOrNameUser))
            {
                response.HasError = true;
                response.Errors.Add("Debe ingresar su correo o nombre de usuario y contraseña.");
                return response;
            }
            if (rolesUser.Contains(Roles.Desarrollador.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("El usuario no tiene permisos para acceder a la aplicación web.");
                return response;
            }
            if (!user.EmailConfirmed || !user.IsActive)
            {
                response.HasError = true;
                response.Errors.Add("El usuario se encuentra inactivo y no puede iniciar sesión.");
                return response;
            }
           
            var verifyUser = await _signInManager.PasswordSignInAsync(user, dto.Password, false, true);
            if (!verifyUser.Succeeded)
            {
                response.HasError = true;
                if (verifyUser.IsLockedOut)
                {
                    response.Errors.Add("Su cuenta se encuentra bloqueada por múltiples intentos fallidos. Intente nuevamente en 15 minutos o restablezca su contraseña.");
                    return response;
                }
                else
                {
                    response.Errors.Add("Los datos de acceso son inválidos.");
                    return response;
                }
            }

            return response;
        }

        private async Task<UserResponseDto> ValidateResentEmailConfirm(UserResponseDto response, AppUsers existUser,
            ResendActivationEmailDto resendActivationEmailDto)
        {
            #region validate 
            if (string.IsNullOrWhiteSpace(resendActivationEmailDto.UserName)
                || string.IsNullOrWhiteSpace(resendActivationEmailDto.Origin))
            {
                response.HasError = true;
                response.Errors.Add("Datos inválidos. Complete la solicitud correctamente.");
                return response;
            }
            if (existUser == null)
            {
                response.HasError = true;
                response.Errors.Add("Debe ingresar un nombre de usuario válido.");
                return response;
            }

            if (string.IsNullOrWhiteSpace(resendActivationEmailDto.Origin))
            {
                response.HasError = true;
                response.Errors.Add("Esta opción no se encuentra disponible en este momento. Intente nuevamente más tarde.");
                return response;
            }

            if (existUser.BlockedEmailSending.HasValue &&
              existUser.BlockedEmailSending!.Value > DateTimeOffset.UtcNow
              )
            {
                response.HasError = true;
                response.Errors.Add("Ya solicitó un correo de confirmación o de cambio de contraseña recientemente. Debe esperar 5 minutos antes de solicitar otro.");
                return response;
            }
            var roles = await _userManager.GetRolesAsync(existUser);
            if (roles.Contains(Roles.Administrador.ToString())
                || roles.Contains(Roles.Desarrollador.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("Esta funcionalidad se encuentra bloqueada para su usuario. Comuníquese con un Administrador.");
                return response;
            }
            #endregion

            return response;
        }

        #endregion
    }
}
