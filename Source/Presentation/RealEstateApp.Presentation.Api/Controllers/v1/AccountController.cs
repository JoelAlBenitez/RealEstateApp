using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Api.Account;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Presentation.Api.Controllers.v1
{

    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        private readonly IAutheAccountWebApi _authAccountWebApi;
        private readonly IOperationalAccountWebApi _operationalAccountWebApi;

        public AccountController(
            IAutheAccountWebApi authAccountWebApi,
            IOperationalAccountWebApi operationalAccountWebApi)
        {
            _authAccountWebApi = authAccountWebApi;
            _operationalAccountWebApi = operationalAccountWebApi;
        }

        
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginApiDtoResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Los datos enviados no son válidos." });
                }

                var result = await _authAccountWebApi.LoginAsync(dto);

                if (result.HasError)
                {
                    if (result.Forbidden)
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, new { errors = result.Errors });
                    }
                    return Unauthorized(new { errors = result.Errors });
                }

                return Ok(new
                {
                    token = result.Token,
                    userName = result.UserName,
                    roles = result.Roles,
                    expiration = result.Expiration
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }
        [Authorize(Roles = "Administrador")]
        [HttpPost("register-developer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterDeveloper([FromBody] RegisterInternalUserApiRequestDto dto)
        {
            return await RegisterInternalUser(dto, Roles.Desarrollador);
        }

      
        [Authorize(Roles = "Administrador")]
        [HttpPost("register-administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterAdministrator([FromBody] RegisterInternalUserApiRequestDto dto)
        {
            return await RegisterInternalUser(dto, Roles.Administrador);
        }

        #region private methods

        private async Task<IActionResult> RegisterInternalUser(RegisterInternalUserApiRequestDto dto, Roles role)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Los datos enviados no son válidos." });
                }

                var register = new RegisterInternalUsersDto
                {
                    Name = dto.Name,
                    LastName = dto.LastName,
                    IDCard = dto.IDCard,
                    Email = dto.Email,
                    NameUser = dto.UserName,
                    Password = dto.Password,
                    ConfirmPassword = dto.ConfirmPassword,
                    TypeUser = (int)role
                };

                var result = await _operationalAccountWebApi.CreateInternalUserAsync(register);

                if (result == null || result.HasError)
                {
                    return BadRequest(new { errors = result?.Errors });
                }

                return Created();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

        #endregion
    }
}
