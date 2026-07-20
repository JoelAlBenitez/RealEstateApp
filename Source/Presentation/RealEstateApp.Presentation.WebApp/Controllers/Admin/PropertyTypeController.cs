using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.PropertyType;
using RealEstateApp.Core.Application.ViewsModel.PropertyType;
using RealEstateApp.Core.Application.DTOs.PropertyType;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Presentation.WebApp.Controllers.Admin
{
    [Authorize(Roles = nameof(Roles.Administrador))]
    public class PropertyTypeController : Controller
    {
        private readonly IPropertyTypeService _propertyTypeService;
        private readonly IMapper _mapper;

        public PropertyTypeController(IPropertyTypeService propertyTypeService, IMapper mapper)
        {
            _propertyTypeService = propertyTypeService;
            _mapper = mapper;
        }

        // GET: /PropertyType
        public async Task<IActionResult> Index()
        {
            var result = await _propertyTypeService.GetAllWithCountAsync();
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Error al cargar los tipos de propiedad.";
                return View(new List<PropertyTypeViewModel>());
            }

            var viewModels = _mapper.Map<List<PropertyTypeViewModel>>(result.Value);
            return View(viewModels);
        }

        // GET: /PropertyType/Create
        public IActionResult Create()
        {
            return View(new CreatePropertyTypeViewModel { Name = string.Empty, Description = string.Empty });
        }

        // POST: /PropertyType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePropertyTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new SavePropertyTypeDto
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };

            var result = await _propertyTypeService.AddAsync(dto);
            if (!result.IsValid)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault()?.Description ?? "Error al crear el tipo de propiedad.");
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Tipo de propiedad creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /PropertyType/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _propertyTypeService.GetByIdAsync(id);
            if (!result.IsValid || result.Value is null)
            {
                TempData["ErrorMessage"] = "No se encontró el tipo de propiedad.";
                return RedirectToAction(nameof(Index));
            }

            // GetByIdAsync returns SavePropertyTypeDto; map it to the edit view model
            var viewModel = new EditPropertyTypeViewModel
            {
                Id = result.Value.Id ?? 0,
                Name = result.Value.Name,
                Description = result.Value.Description
            };
            return View(viewModel);
        }

        // POST: /PropertyType/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPropertyTypeViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new SavePropertyTypeDto
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Description = viewModel.Description
            };

            var result = await _propertyTypeService.UpdateAsync(dto);
            if (result is not null && !result.IsValid)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault()?.Description ?? "Error al actualizar el tipo de propiedad.");
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Tipo de propiedad actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /PropertyType/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _propertyTypeService.RemoveAsync(id);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Error al eliminar el tipo de propiedad.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Tipo de propiedad eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
