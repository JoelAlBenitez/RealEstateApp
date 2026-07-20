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
        private readonly IAgentPropertyService _agentPropertyService;
        private readonly IOperationalAccountWebApp _accountService;
        private readonly IUserSession _userSession;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;

        public AgentController(
            IAgentPropertyService agentPropertyService,
            IOperationalAccountWebApp accountService,
            IUserSession userSession,
            IFileManager fileManager,
            IMapper mapper)
        {
            _agentPropertyService = agentPropertyService;
            _accountService = accountService;
            _userSession = userSession;
            _fileManager = fileManager;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 6)
        {
            if (pageNumber < 1) pageNumber = 1;

            var agentId = _userSession.GetIdCurrentUser();
            var countResult = await _agentPropertyService.CountByAgentAsync(agentId);
            var totalItems = countResult.IsValid ? countResult.Value : 0;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            var result = await _agentPropertyService.GetPropertiesByAgentAsync(agentId, pageNumber, pageSize);
            
            if (!result.IsValid)
            {
                ViewBag.CurrentPage = 1;
                ViewBag.TotalPages = 1;
                ViewBag.TotalItems = 0;
                return View(new List<PropertyCardViewModel>());
            }

            var viewModels = _mapper.Map<List<PropertyCardViewModel>>(result.Value);

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = totalItems;

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
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault() ?? "Ocurrió un error al actualizar el perfil.";
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
