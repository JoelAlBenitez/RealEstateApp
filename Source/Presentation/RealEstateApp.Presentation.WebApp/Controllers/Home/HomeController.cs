using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.ViewsModel.Property;

namespace RealEstateApp.Presentation.WebApp.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IMapper _mapper;

        public HomeController(IPropertyService propertyService,
            IMapper mapper)
        {
            _propertyService = propertyService;
            _mapper = mapper;
        }

        #region methods load
        public async Task<IActionResult> Index()
        {
            var propertyes = await _propertyService.GetAllAsync();
            if (!propertyes.IsValid)
            {
                foreach (var item in propertyes.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            var map = _mapper.Map<IReadOnlyCollection<PropertyPublicViewModel>>(propertyes.Value);
            return View(map);
        }
        public IActionResult ViewSpecificProperty(PropertyPublicViewModel property) { 

           if(!ModelState.IsValid || property == null)
            {
                ModelState.AddModelError("", "La propiedad que a consultado no se encuentra disponible. Favor intente de nuevo.");
                return View(nameof(Index));
            }
           return View(property);
        }
        public IActionResult FilterProperty()
        {
            return View(new PropertyFilterViewModel
            {
                //agregar campos extras cuando adrian y sebastian terminen

                Bathrooms = 0,
                IdTypeProperty = 0,
                MaxPrice = 0,
                MinPrice = 0,
                TypePropery = null
            });
        }
        
        public async Task<IActionResult> DetailtProperty(int IdProperty)
        {
            var result = await _propertyService.GetByIdWithDetailsAsync(IdProperty);
            if(!result.IsValid)
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return View(nameof(Index));
            }
            var map = _mapper.Map<Proper>

        }
        #endregion

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ConsultByCode(PropertySearchByCodeViewModel code)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Favor complete todos los campos del formulario.");
                return View(nameof(Index));
            }
            var consult = await _propertyService.GetByCodeAsync(code.Code);
            if (!consult.IsValid)
            {
                foreach (var item in consult.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return View(nameof(Index));
            }
            var map = _mapper.Map<PropertyPublicViewModel>(consult.Value);
            return View("ViewSpecificProperty", map);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> FilterProperty(PropertyFilterViewModel vm)
        {
            if (!ModelState.IsValid) {

                ModelState.AddModelError("", "La busqueda no pudo ser realizada");
                return View(nameof(Index));
            }
            var map = _mapper.Map<PropertyFilterDto>(vm);
            var result = await _propertyService.GetAvailableAsync(map);
            if(!result.IsValid)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
                return View(nameof(Index));
            }
            var map2 = _mapper.Map<IReadOnlyCollection<PropertyFilterViewModel>>(result.Value);
            return View("Index", map2);
        }

        
    }
}
