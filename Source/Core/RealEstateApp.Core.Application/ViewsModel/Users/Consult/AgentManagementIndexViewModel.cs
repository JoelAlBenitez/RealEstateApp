namespace RealEstateApp.Core.Application.ViewsModel.Users.Consult
{
    // Contenedor de la pantalla "Listado de los agentes" (administrador).
    // Evita el uso de ViewBag: filtro activo, conteos, paginación y modo de visualización viajan aquí.
    public sealed class AgentManagementIndexViewModel
    {
        public IReadOnlyList<AgentListItemViewModel> Agents { get; set; } = new List<AgentListItemViewModel>();

        public AgentListFilter Filter { get; set; } = AgentListFilter.Todos;

        // Total de agentes que corresponden al filtro actual (antes de paginar).
        public int TotalForFilter { get; set; }

        // Conteos para las pestañas de filtro.
        public int CountAll { get; set; }
        public int CountActive { get; set; }
        public int CountInactive { get; set; }
        public int CountPending { get; set; }

        // Paginación.
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages { get; set; } = 1;

        // Solo se permite alternar entre tabla y tarjetas cuando el filtro tiene 10 o menos agentes.
        public bool CanUseCards => TotalForFilter > 0 && TotalForFilter <= PageSize;
    }
}
