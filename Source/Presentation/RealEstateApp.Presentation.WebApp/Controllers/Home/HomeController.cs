using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Common;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Domain.Common.Errors;

namespace RealEstateApp.Presentation.WebApp.Controllers.Home
{
    public class HomeController : Controller
    {
        private const int PageSize = 12;

        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApp _operationalAccountWebApp;
        private readonly IMapper _mapper;

        public HomeController(
            IPropertyService propertyService,
            IOperationalAccountWebApp operationalAccountWebApp,
            IMapper mapper
            )
        {
            _propertyService = propertyService;
            _operationalAccountWebApp = operationalAccountWebApp;
            _mapper = mapper;
        }

        #region methods load

        #region methods properties public
        public async Task<IActionResult> Index(PropertyFilterViewModel filter, int page = 1)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "La busqueda no pudo ser realizada. Favor revise los valores ingresados.");
                return View(await BuildHomeAsync(null, new PropertyFilterViewModel(), 1, false));
            }
            var isFiltered = HasFilters(filter);
            var filters = isFiltered ? _mapper.Map<PropertyFilterDto>(filter) : null;
            return View(await BuildHomeAsync(filters, filter, page, isFiltered));
        }

        public async Task<IActionResult> DetailtsProperty(int IdProperty)
        {
            var result = await _propertyService.GetByIdWithDetailsAsync(IdProperty);
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
                return View("Index", await BuildHomeAsync(null, new PropertyFilterViewModel(), 1, false));
            }
            var map = _mapper.Map<PropertyDetailViewModel>(result.Value);
            await LoadAgentContactAsync(map);
            return View("DetailsProperty", map);
        }

        public IActionResult Error()
        {
            return View("ErrorPage", new ErrorPageViewModel
            {
                Code = 500,
                Title = "Algo salió mal",
                Message = "Ha ocurrido un error inesperado al procesar la solicitud. Favor intente nuevamente más tarde."
            });
        }

        public IActionResult StatusCodeError(int code)
        {
            var page = code == 404
                ? new ErrorPageViewModel
                {
                    Code = 404,
                    Title = "Página no encontrada",
                    Message = "La página que busca no existe o fue movida. Verifique la dirección o vuelva al inicio."
                }
                : new ErrorPageViewModel
                {
                    Code = code,
                    Title = "Algo salió mal",
                    Message = "Ha ocurrido un error inesperado al procesar la solicitud. Favor intente nuevamente más tarde."
                };
            return View("ErrorPage", page);
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
                return View("Index", await BuildHomeAsync(null, new PropertyFilterViewModel(), 1, false));
            }
            var consult = await _propertyService.GetByCodeAsync(vm.Code.Trim());
            if (!consult.IsValid || consult.Value == null)
            {
                AddErrors(consult.Errors);
                return View("Index", await BuildHomeAsync(null, new PropertyFilterViewModel(), 1, false));
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
                return View("Index", await BuildHomeAsync(null, new PropertyFilterViewModel(), 1, false));
            }
            return RedirectToAction(nameof(Index), new
            {
                vm.MinPrice,
                vm.MaxPrice,
                vm.Bedrooms,
                vm.Bathrooms,
                vm.IdTypeProperty
            });
        }

        #endregion

        #region private helpers
        private static bool HasFilters(PropertyFilterViewModel filter)
        {
            return filter.MinPrice.HasValue
                || filter.MaxPrice.HasValue
                || filter.Bedrooms.HasValue
                || filter.Bathrooms.HasValue
                || filter.IdTypeProperty.HasValue;
        }

        private async Task<HomePropertiesViewModel> BuildHomeAsync(
            PropertyFilterDto? filters,
            PropertyFilterViewModel filter,
            int page,
            bool isFiltered)
        {
            var totalPages = 1;
            var countResult = await _propertyService.CountAvailableAsync(filters);
            if (!countResult.IsValid)
            {
                AddErrors(countResult.Errors);
            }
            else
            {
                totalPages = Math.Max(1, (int)Math.Ceiling(countResult.Value / (double)PageSize));
            }
            page = Math.Clamp(page, 1, totalPages);

            IReadOnlyCollection<PropertyPublicViewModel> properties = Array.Empty<PropertyPublicViewModel>();
            var result = await _propertyService.GetAvailableAsync(filters, page, PageSize);
            if (!result.IsValid)
            {
                AddErrors(result.Errors);
            }
            else
            {
                properties = _mapper.Map<IReadOnlyCollection<PropertyPublicViewModel>>(result.Value)
                    ?? Array.Empty<PropertyPublicViewModel>();
            }

            return new HomePropertiesViewModel
            {
                Properties = properties,
                Filter = filter,
                SearchByCode = new PropertySearchByCodeViewModel { Code = "" },
                Page = page,
                PageSize = PageSize,
                TotalPages = totalPages,
                IsFiltered = isFiltered
            };
        }

        private async Task LoadAgentContactAsync(PropertyDetailViewModel detail)
        {
            if (string.IsNullOrWhiteSpace(detail.AgentId))
            {
                return;
            }
            var agent = await _operationalAccountWebApp.GetConsultAgentById(detail.AgentId);
            if (agent == null)
            {
                return;
            }
            detail.AgentName = $"{agent.Name} {agent.LastName}";
            detail.AgentPhone = agent.PhoneNumber;
            detail.AgentEmail = agent.Email;
            detail.AgentPhotoUrl = agent.ProfileImgAgent;
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
