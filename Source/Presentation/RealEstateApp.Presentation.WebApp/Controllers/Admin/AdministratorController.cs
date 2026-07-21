using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Admin;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Presentation.WebApp.Controllers.Admin
{
    [Authorize(Roles = "Administrador")]
    public class AdministratorController : Controller
    {
        private readonly IAdministratorService _administratorService;
        private readonly IOperationalAccountWebApi _internalAccountApi;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public AdministratorController(
            IAdministratorService administratorService,
            IOperationalAccountWebApi internalAccountApi,
            IUserSession userSession,
            IMapper mapper)
        {
            _administratorService = administratorService;
            _internalAccountApi = internalAccountApi;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _administratorService.GetAdministratorsAsync();
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al cargar el listado de administradores.";
                return View(new List<AdministratorListItemViewModel>());
            }

            var viewModels = _mapper.Map<List<AdministratorListItemViewModel>>(result.Value);
            return View(viewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateInternalUserViewModel
            {
                Name = string.Empty,
                LastName = string.Empty,
                Email = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                NameUser = string.Empty,
                IDCard = string.Empty,
                TypeUser = (int)Roles.Administrador
            };
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        
        public async Task<IActionResult> Create(CreateInternalUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = _mapper.Map<RegisterInternalUsersDto>(vm);
            dto.TypeUser = (int)Roles.Administrador;

            var result = await _internalAccountApi.CreateInternalUserAsync(dto);
            if (result.HasError)
            {
                foreach (var error in result.Errors ?? new List<string>())
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(vm);
            }

            TempData["SuccessMessage"] = "El administrador fue creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var result = await _administratorService.GetAdministratorsAsync();
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al intentar editar el administrador.";
                return RedirectToAction(nameof(Index));
            }

            var admin = result.Value?.FirstOrDefault(a => a.Id == id);
            if (admin == null)
            {
                return NotFound();
            }

            var vm = _mapper.Map<EditInternalUserViewModel>(admin);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditInternalUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var dto = _mapper.Map<EditInternalUserDto>(vm);

            var result = await _internalAccountApi.UpdateInternalUserAsync(dto);
            if (result.HasError)
            {
                foreach (var error in result.Errors ?? new List<string>())
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(vm);
            }

            TempData["SuccessMessage"] = "El administrador fue actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id, bool active)
        {
            var currentAdminId = _userSession.GetIdCurrentUser();
            var dto = new AlterStateUserDto
            {
                Id = id,
                State = active
            };

            var result = await _administratorService.ToggleStatusAsync(dto, currentAdminId);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al cambiar el estado del administrador.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = active
                ? "El administrador fue activado correctamente."
                : "El administrador fue inactivado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}
