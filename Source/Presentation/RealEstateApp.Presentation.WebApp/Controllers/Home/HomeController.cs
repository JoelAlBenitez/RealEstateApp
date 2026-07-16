using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.Properties;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Property;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser;
using RealEstateApp.Core.Application.ViewsModel.Property;
using RealEstateApp.Core.Application.ViewsModel.Users.Consult;

namespace RealEstateApp.Presentation.WebApp.Controllers.Home
{
    public class HomeController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IOperationalAccountWebApp _operationalAccountWebApp;
        private readonly IMapper _mapper;

        public HomeController(
            IPropertyService propertyService,
            IMapper mapper,
            IOperationalAccountWebApp operationalAccountWebApp
            )
        {
            _propertyService = propertyService;
            _mapper = mapper;
            _operationalAccountWebApp = operationalAccountWebApp;
        }

        #region methods load

        #region methods properties public
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
            return PartialView(new PropertyFilterViewModel
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
            var map = _mapper.Map<PropertyDetailViewModel>(result.Value);
            return View(map);

        }
        public  IActionResult ConsultByCode()
        {
            return PartialView(new PropertySearchByCodeViewModel { Code = "" });
        }
        #endregion

        #region methods agents public
        public async Task<IActionResult> Agents()
        {
            var result = await _operationalAccountWebApp.GetAgentAllViewHomeByCustomer();
            var map = _mapper.Map<IReadOnlyCollection<AgentViewModel>>(result);
            return View(map);
        }
        public IActionResult AgentsConsult()
        {
            return PartialView(new AgentConsultByNameOrLastNameViewModel
            {
                Name = ""
            });
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
                ModelState.AddModelError("", "Favor complete todos los campos del formulario.");
                return View(nameof(Index));
            }
            var consult = await _propertyService.GetByCodeAsync(vm.Code);
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AgentsConsult(AgentConsultByNameOrLastNameViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Favor complete los camos pertinentes del formulario.");
                return View(nameof(Agents));
            }
            var map = _mapper.Map<ConsultAgentByNameOrLastNameDto>(vm);
            var result = await _operationalAccountWebApp.GetAgentByConsultCustomer(map);
            if(result == null)
            {
                ModelState.AddModelError("", "No se encontraron agentes activos con el nombre ingresado.");
                return View(nameof(Agents));
            }
            var map2 = _mapper.Map<IReadOnlyCollection<AgentViewModel>>(result);
            return View("Agents", map2);

        }
        #endregion

    }
}
