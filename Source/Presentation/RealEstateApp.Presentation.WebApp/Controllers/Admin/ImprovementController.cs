using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Improvement;
using RealEstateApp.Core.Application.DTOs.Improvement;
using RealEstateApp.Core.Application.ViewsModel.Improvement;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateApp.Presentation.WebApp.Controllers.Admin
{
    [Authorize(Roles = "Administrador")]
    public class ImprovementController : Controller
    {
        private readonly IImprovementService _improvementService;
        private readonly IMapper _mapper;

        public ImprovementController(IImprovementService improvementService, IMapper mapper)
        {
            _improvementService = improvementService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _improvementService.GetAllWithCountAsync();
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "No se pudieron cargar las mejoras en este momento.";
                return View(new List<ImprovementViewModel>());
            }

            var viewModels = _mapper.Map<List<ImprovementViewModel>>(result.Value);
            return View(viewModels);
        }

        public IActionResult Create()
        {
            return View("SaveImprovement", new SaveImprovementViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SaveImprovementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImprovement", model);
            }

            var dto = _mapper.Map<SaveImprovementDto>(model);
            var result = await _improvementService.AddAsync(dto);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("SaveImprovement", model);
            }

            TempData["SuccessMessage"] = "Mejora creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _improvementService.GetByIdAsync(id);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "No se pudo obtener la mejora seleccionada.";
                return RedirectToAction(nameof(Index));
            }

            var model = _mapper.Map<SaveImprovementViewModel>(result.Value);
            return View("SaveImprovement", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SaveImprovementViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveImprovement", model);
            }

            var dto = _mapper.Map<SaveImprovementDto>(model);
            var result = await _improvementService.UpdateAsync(dto);

            if (!result!.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View("SaveImprovement", model);
            }

            TempData["SuccessMessage"] = "Mejora actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _improvementService.RemoveAsync(id);
            if (!result.IsValid)
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la mejora en este momento.";
            }
            else
            {
                TempData["SuccessMessage"] = "Mejora eliminada correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
