using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Improvement;
using RealEstateApp.Core.Application.DTOs.Api.Improvements;
using RealEstateApp.Core.Application.DTOs.Improvement;

namespace RealEstateApp.Presentation.Api.Controllers.v1
{
    
    [ApiVersion("1.0")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public class ImprovementController : BaseApiController
    {
        private readonly IImprovementService _improvementService;

        public ImprovementController(IImprovementService improvementService)
        {
            _improvementService = improvementService;
        }

        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImprovementApiDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {
            try
            {
                var result = await _improvementService.GetAllAsync();

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
                    .Select(x => new ImprovementApiDto
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementApiDto))]
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

                var result = await _improvementService.GetByIdAsync(id);

                if (!result.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }

                if (result.Value == null)
                {
                    return NotFound(new { message = "La mejora solicitada no existe." });
                }

                return Ok(new ImprovementApiDto
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
        public async Task<IActionResult> Create([FromBody] SaveImprovementApiRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Los datos enviados no son válidos." });
                }

                var result = await _improvementService.AddAsync(new SaveImprovementDto
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImprovementApiDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] SaveImprovementApiRequestDto dto)
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

                var existing = await _improvementService.GetByIdAsync(id);
                if (!existing.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }
                if (existing.Value == null)
                {
                    return NotFound(new { message = "La mejora solicitada no existe." });
                }

                var result = await _improvementService.UpdateAsync(new SaveImprovementDto
                {
                    Id = id,
                    Name = dto.Name,
                    Description = dto.Description
                });

                if (result != null && !result.IsValid)
                {
                    return BadRequest(new { message = result.Errors.FirstOrDefault()?.Description ?? "Los datos enviados no son válidos." });
                }

                return Ok(new ImprovementApiDto
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

                var existing = await _improvementService.GetByIdAsync(id);
                if (!existing.IsValid)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." });
                }
                if (existing.Value == null)
                {
                    return NotFound(new { message = "La mejora solicitada no existe." });
                }

                var result = await _improvementService.RemoveAsync(id);
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
