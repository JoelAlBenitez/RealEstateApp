using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Api;
using RealEstateApp.Core.Application.DTOs.Api.Agents;
using RealEstateApp.Core.Application.DTOs.Api.Properties;

namespace RealEstateApp.Presentation.Api.Controllers.v1
{
  
    [ApiVersion("1.0")]
    [Authorize(Roles = "Administrador,Desarrollador")]
    public class AgentController : BaseApiController
    {
        private readonly IAgentApiService _agentApiService;
        private readonly IMapper _mapper;

        public AgentController(IAgentApiService agentApiService, IMapper mapper)
        {
            _agentApiService = agentApiService;
            _mapper = mapper;
        }

        
        [HttpGet("List")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AgentApiDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {
            try
            {
                var agents = await _agentApiService.GetAllAsync();

                if (agents == null || agents.Count == 0)
                {
                    return NoContent();
                }

                return Ok(agents);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

      
        [HttpGet("GetById/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AgentApiDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "El Id enviado no tiene un formato válido." });
                }

                var agent = await _agentApiService.GetByIdAsync(id);

                if (agent == null)
                {
                    return NotFound(new { message = "El agente solicitado no existe." });
                }

                return Ok(agent);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }

       
        [HttpGet("GetAgentProperty/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PropertyApiDto>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAgentProperty(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "El Id enviado no tiene un formato válido." });
                }

                var result = await _agentApiService.GetAgentPropertiesAsync(id);

                if (result == null)
                {
                    return NotFound(new { message = "El agente solicitado no existe." });
                }

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

       
        [Authorize(Roles = "Administrador")]
        [HttpPatch("ChangeStatus/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] ChangeAgentStatusApiDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return BadRequest(new { message = "El Id enviado no tiene un formato válido." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "El estado enviado no es válido." });
                }

                var result = await _agentApiService.ChangeStatusAsync(id, dto.Status);

                return result switch
                {
                    ChangeAgentStatusResult.Success => NoContent(),
                    ChangeAgentStatusResult.NotFound => NotFound(new { message = "El agente solicitado no existe." }),
                    _ => StatusCode(StatusCodes.Status500InternalServerError,
                        new { message = "Ocurrió un error interno en el servidor." })
                };
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Ocurrió un error interno en el servidor." });
            }
        }
    }
}
