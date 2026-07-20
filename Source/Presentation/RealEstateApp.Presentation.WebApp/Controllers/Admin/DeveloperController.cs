using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Admin;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.InternaUser;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Presentation.WebApp.Controllers.Admin
{
    [Authorize(Roles = "Administrador")]
    public class DeveloperController : Controller
    {
        private readonly IDeveloperService _developerService;
        private readonly IOperationalAccountWebApi _internalAccountApi;
        private readonly IMapper _mapper;

        public DeveloperController(
            IDeveloperService developerService,
            IOperationalAccountWebApi internalAccountApi,
            IMapper mapper)
        {
            _developerService = developerService;
            _internalAccountApi = internalAccountApi;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _developerService.GetDevelopersAsync();
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al cargar el listado de desarrolladores.";
                return View(new List<DeveloperListItemViewModel>());
            }

            var viewModels = _mapper.Map<List<DeveloperListItemViewModel>>(result.Value);
            return View(viewModels);
        }

        [ValidateAntiForgeryToken]
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
                TypeUser = (int)Roles.Desarrollador
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(CreateInternalUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var registerDto = _mapper.Map<RegisterInternalUsersDto>(viewModel);
            var response = await _internalAccountApi.CreateInternalUserAsync(registerDto);

            if (response.HasError)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "El desarrollador fue creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var devsResult = await _developerService.GetDevelopersAsync();
            if (!devsResult.IsValid)
            {
                TempData["ErrorMessage"] = "No se pudo obtener la información del desarrollador.";
                return RedirectToAction(nameof(Index));
            }

            var devDto = devsResult.Value!.FirstOrDefault(d => d.Id == id);
            if (devDto == null)
            {
                TempData["ErrorMessage"] = "El desarrollador solicitado no existe.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = _mapper.Map<EditInternalUserViewModel>(devDto);
            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(EditInternalUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var editDto = _mapper.Map<EditInternalUserDto>(viewModel);
            var response = await _internalAccountApi.UpdateInternalUserAsync(editDto);

            if (response.HasError)
            {
                foreach (var error in response.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "El desarrollador fue actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id, bool active)
        {
            var dto = new AlterStateUserDto
            {
                Id = id,
                State = active
            };

            var result = await _developerService.ToggleStatusAsync(dto);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Ocurrió un error al cambiar el estado del desarrollador.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = active 
                ? "El desarrollador fue activado correctamente." 
                : "El desarrollador fue inactivado correctamente.";

            return RedirectToAction(nameof(Index));
        }
    }
}
