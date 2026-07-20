using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.SaleType;
using RealEstateApp.Core.Application.ViewsModel.SaleType;
using RealEstateApp.Core.Application.DTOs.SaleType;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Presentation.WebApp.Controllers.Admin
{
    [Authorize(Roles = nameof(Roles.Administrador))]
    public class SaleTypeController : Controller
    {
        private readonly ISaleTypeService _saleTypeService;
        private readonly IMapper _mapper;

        public SaleTypeController(ISaleTypeService saleTypeService, IMapper mapper)
        {
            _saleTypeService = saleTypeService;
            _mapper = mapper;
        }

        // GET: /SaleType
        public async Task<IActionResult> Index()
        {
            var result = await _saleTypeService.GetAllWithCountAsync();
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Error al cargar los tipos de venta.";
                return View(new List<SaleTypeViewModel>());
            }

            var viewModels = _mapper.Map<List<SaleTypeViewModel>>(result.Value);
            return View(viewModels);
        }

        // GET: /SaleType/Create
        public IActionResult Create()
        {
            return View(new SaveSaleTypeViewModel { Name = string.Empty, Description = string.Empty });
        }

        // POST: /SaleType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveSaleTypeViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new SaveSaleTypeDto
            {
                Name = viewModel.Name ?? string.Empty,
                Description = viewModel.Description ?? string.Empty
            };

            var result = await _saleTypeService.AddAsync(dto);
            if (!result.IsValid)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault()?.Description ?? "Error al crear el tipo de venta.");
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Tipo de venta creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /SaleType/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _saleTypeService.GetByIdAsync(id);
            if (!result.IsValid || result.Value is null)
            {
                TempData["ErrorMessage"] = "No se encontró el tipo de venta.";
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new SaveSaleTypeViewModel
            {
                Id = result.Value.Id ?? 0,
                Name = result.Value.Name,
                Description = result.Value.Description
            };
            return View(viewModel);
        }

        // POST: /SaleType/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SaveSaleTypeViewModel viewModel)
        {
            if (id != viewModel.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(viewModel);

            var dto = new SaveSaleTypeDto
            {
                Id = viewModel.Id,
                Name = viewModel.Name ?? string.Empty,
                Description = viewModel.Description ?? string.Empty
            };

            var result = await _saleTypeService.UpdateAsync(dto);
            if (result is not null && !result.IsValid)
            {
                ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault()?.Description ?? "Error al actualizar el tipo de venta.");
                return View(viewModel);
            }

            TempData["SuccessMessage"] = "Tipo de venta actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        // POST: /SaleType/Delete/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _saleTypeService.RemoveAsync(id);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = result.Errors.FirstOrDefault()?.Description ?? "Error al eliminar el tipo de venta.";
                return RedirectToAction(nameof(Index));
            }

            TempData["SuccessMessage"] = "Tipo de venta eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
