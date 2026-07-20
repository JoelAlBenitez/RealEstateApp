using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Api.Properties;
using System.Text.RegularExpressions;

namespace RealEstateApp.Presentation.Api.Controllers.v1
{
    
    [ApiVersion("1.0")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public class PropertyController : BaseApiController
    {
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public PropertyController(IPropertyService propertyService, IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyApiDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {
            try
            {
                var result = await _propertyService.GetAllForApiAsync();

                if (!result.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }

                if (result.Value == null || result.Value.Count == 0)
                {
                    return NoContent();
                }

                var properties = _mapper.Map<List<PropertyApiDto>>(result.Value);
                return Ok(properties);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

      
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyApiDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El Id enviado no tiene un formato válido." });
                }

                var result = await _propertyService.GetByIdWithDetailsAsync(id);

                if (!result.IsValid)
                {
                    if (result.Errors.Any(e => e.Code == "Property.NotFound"))
                    {
                        return NotFound(new { message = "La propiedad solicitada no existe." });
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }

                var property = _mapper.Map<PropertyApiDto>(result.Value);
                return Ok(property);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

     
        [HttpGet("GetByCode/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyApiDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByCode(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code) || !Regex.IsMatch(code.Trim(), @"^\d{6}$"))
                {
                    return BadRequest(new { message = "El código enviado no tiene un formato válido." });
                }

                var result = await _propertyService.GetByCodeForApiAsync(code.Trim());

                if (!result.IsValid)
                {
                    if (result.Errors.Any(e => e.Code == "Property.NotFound"))
                    {
                        return NotFound(new { message = "No existe una propiedad registrada con el código enviado." });
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }

                var property = _mapper.Map<PropertyApiDto>(result.Value);
                return Ok(property);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }
    }
}
