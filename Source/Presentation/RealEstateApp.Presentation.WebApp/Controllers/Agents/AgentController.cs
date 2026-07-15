using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.Contracts.FileManager;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser;
using AutoMapper;

namespace RealEstateApp.Presentation.WebApp.Controllers.Agents
{
    [Authorize(Roles = "Agente")]
    public class AgentController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApp _accountService;
        private readonly IUserSession _userSession;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;

        public AgentController(
            IPropertyService propertyService,
            IOperationalAccountWebApp accountService,
            IUserSession userSession,
            IFileManager fileManager,
            IMapper mapper)
        {
            _propertyService = propertyService;
            _accountService = accountService;
            _userSession = userSession;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var agentId = _userSession.GetIdCurrentUser();
            var result = await _propertyService.GetPropertiesByAgentAsync(agentId, pageNumber, pageSize);
            
            if (!result.IsValid)
            {
                return View(new List<PropertyCardViewModel>());
            }

            var viewModels = _mapper.Map<List<PropertyCardViewModel>>(result.Value);
            return View(viewModels);
        }

        public async Task<IActionResult> Profile()
        {
            var agentId = _userSession.GetIdCurrentUser();
            var result = await _accountService.GetConsultAgentById(agentId);
            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new EditExternalUserViewModel
            {
                Id = result.Id,
                Name = result.Name,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                ProfileImgCurrent = result.ProfileImgAgent
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(EditExternalUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var editDto = new EditAgentUserDto
            {
                Id = model.Id,
                Name = model.Name,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                ProfileImg = model.ProfileImgCurrent,
                ChangePorfileImg = false
            };

            if (model.NewProfileImage != null)
            {
                var newImgPath = await _fileManager.SaveAsync(model.NewProfileImage, "Users", model.Id);
                if (!string.IsNullOrEmpty(newImgPath))
                {
                    editDto.ProfileImg = newImgPath;
                    editDto.ChangePorfileImg = true;
                }
            }

            var result = await _accountService.UpdateAgentAsync(editDto);
            if (result.HasError)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al actualizar el perfil.";
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
