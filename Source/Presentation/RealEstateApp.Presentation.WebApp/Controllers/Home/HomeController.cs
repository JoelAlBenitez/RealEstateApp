using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Presentation.WebApp.Controllers.Home
{
    public class HomeController : Controller
    {
        private const int PageSize = 12;

        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public HomeController(
            IPropertyService propertyService,
            IMapper mapper
            )
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        #region methods load

        #region methods properties public
        public async Task<IActionResult> Index(int page = 1)
        {
            return View(await BuildHomeAsync(null, page));
        }

        public async Task<IActionResult> DetailtsProperty(int IdProperty)
        {
            var result = await _propertyService.GetByIdWithDetailsAsync(IdProperty);
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
                return View("Index", await BuildHomeAsync(null, 1));
            }
            var map = _mapper.Map<PropertyDetailViewModel>(result.Value);
            return View("DetailsProperty", map);
        }
        #endregion

        #endregion

        #region methods posts
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ConsultByCode(PropertySearchByCodeViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", await BuildHomeAsync(null, 1));
            }
            var consult = await _propertyService.GetByCodeAsync(vm.Code);
            if (!consult.IsValid || consult.Value == null)
            {
                AddErrors(consult.Errors);
                return View("Index", await BuildHomeAsync(null, 1));
            }
            return RedirectToAction(nameof(DetailtsProperty), new { IdProperty = consult.Value.Id });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> FilterProperty(PropertyFilterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "La busqueda no pudo ser realizada. Favor revise los valores ingresados.");
                return View("Index", await BuildHomeAsync(null, 1));
            }
            var filters = _mapper.Map<PropertyFilterDto>(vm);
            var result = await BuildHomeAsync(filters, 1, false);
            result.Filter = vm;
            result.IsFiltered = true;
            return View("Index", result);
        }

        #endregion

        #region private helpers
        private async Task<HomePropertiesViewModel> BuildHomeAsync(PropertyFilterDto? filters, int page, bool paginateServerSide = true)
        {
            var result = await _propertyService.GetAvailableAsync(filters);
            IReadOnlyCollection<PropertyPublicViewModel> properties = Array.Empty<PropertyPublicViewModel>();
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
            }
            else
            {
                properties = _mapper.Map<IReadOnlyCollection<PropertyPublicViewModel>>(result.Value)
                    ?? Array.Empty<PropertyPublicViewModel>();
            }
            return BuildHomeViewModel(properties, new PropertyFilterViewModel(), page, paginateServerSide);
        }

        private static HomePropertiesViewModel BuildHomeViewModel(
            IReadOnlyCollection<PropertyPublicViewModel> properties,
            PropertyFilterViewModel filter,
            int page,
            bool paginateServerSide)
        {
            // HANDOFF: cuando el otro dev entregue el servicio de tipos de propiedad,
            // cargar aquí filter.TypePropery con el listado de TypePropertyViewModel para
            // que el select del formulario de filtro (_PropertyFilter) se llene solo.
            var totalPages = Math.Max(1, (int)Math.Ceiling(properties.Count / (double)PageSize));
            if (!paginateServerSide)
            {
                return new HomePropertiesViewModel
                {
                    Properties = properties,
                    Filter = filter,
                    SearchByCode = new PropertySearchByCodeViewModel { Code = "" },
                    Page = 1,
                    PageSize = PageSize,
                    TotalPages = 1
                };
            }
            page = Math.Clamp(page, 1, totalPages);
            var items = properties.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            return new HomePropertiesViewModel
            {
                Properties = items,
                Filter = filter,
                SearchByCode = new PropertySearchByCodeViewModel { Code = "" },
                Page = page,
                PageSize = PageSize,
                TotalPages = totalPages
            };
        }

        private void AddErrors(IReadOnlyCollection<Error> errors)
        {
            foreach (var item in errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }
        }
        #endregion
    }
}
