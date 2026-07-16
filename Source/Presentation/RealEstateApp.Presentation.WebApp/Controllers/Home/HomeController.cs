using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
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
                ModelState.AddModelError("", "Al parecer este apartado se encuentra en mantenimiento. Favor regresar más tarde.");
                return View();
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
                ModelState.AddModelError("", "No se encontró ninguna propiedad disponible con el código ingresado.");
                return View(nameof(Index));
            }
            var map = _mapper.Map<PropertyPublicViewModel>(consult.Value);
            return ViewSpecificProperty(map);
        }


    }
}
