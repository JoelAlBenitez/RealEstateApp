using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.DTOs.Api.PropertyTypes;
using RealEstateApp.Core.Application.DTOs.PropertyType;

namespace RealEstateApp.Presentation.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public class PropertyTypeController : BaseApiController
    {
        private readonly IPropertyTypeService _propertyTypeService;

        public PropertyTypeController(IPropertyTypeService propertyTypeService)
        {
            _propertyTypeService = propertyTypeService;
        }

        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyTypeApiDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {
            try
            {
                var result = await _propertyTypeService.GetAllAsync();

                if (!result.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }

                if (result.Value == null || result.Value.Count == 0)
                {
                    return NoContent();
                }

                var dtos = result.Value
                    .Select(x => new PropertyTypeApiDto
                    {
                        Id = x.Id ?? 0,
                        Name = x.Name,
                        Description = x.Description
                    })
                    .ToList();

                return Ok(dtos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeApiDto))]
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

                var result = await _propertyTypeService.GetByIdAsync(id);

                if (!result.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }

                if (result.Value == null)
                {
                    return NotFound(new { message = "El tipo de propiedad solicitado no existe." });
                }

                return Ok(new PropertyTypeApiDto
                {
                    Id = result.Value.Id ?? 0,
                    Name = result.Value.Name,
                    Description = result.Value.Description
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] SavePropertyTypeApiRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Los datos enviados no son válidos." });
                }

                var result = await _propertyTypeService.AddAsync(new SavePropertyTypeDto
                {
                    Name = dto.Name,
                    Description = dto.Description
                });

                if (!result.IsValid)
                {
                    return BadRequest(new { message = result.Errors.FirstOrDefault()?.Description ?? "Los datos enviados no son válidos." });
                }

                return Created();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpPut("Update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PropertyTypeApiDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] SavePropertyTypeApiRequestDto dto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El Id enviado no tiene un formato válido." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Los datos enviados no son válidos." });
                }

                var existing = await _propertyTypeService.GetByIdAsync(id);
                if (!existing.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }
                if (existing.Value == null)
                {
                    return NotFound(new { message = "El tipo de propiedad solicitado no existe." });
                }

                var result = await _propertyTypeService.UpdateAsync(new SavePropertyTypeDto
                {
                    Id = id,
                    Name = dto.Name,
                    Description = dto.Description
                });

                if (result != null && !result.IsValid)
                {
                    return BadRequest(new { message = result.Errors.FirstOrDefault()?.Description ?? "Los datos enviados no son válidos." });
                }

                return Ok(new PropertyTypeApiDto
                {
                    Id = id,
                    Name = dto.Name,
                    Description = dto.Description
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("Delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "El Id enviado no tiene un formato válido." });
                }

                var existing = await _propertyTypeService.GetByIdAsync(id);
                if (!existing.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }
                if (existing.Value == null)
                {
                    return NotFound(new { message = "El tipo de propiedad solicitado no existe." });
                }

                var result = await _propertyTypeService.RemoveAsync(id);
                if (!result.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }
    }
}
