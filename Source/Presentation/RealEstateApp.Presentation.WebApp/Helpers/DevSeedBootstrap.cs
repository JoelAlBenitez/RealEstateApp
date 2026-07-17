namespace RealEstateApp.Presentation.WebApp.Helpers
{
    // Puente hacia el seed local de pruebas (folder Seeds, ignorado por git).
    // Se invoca por reflexión para que el proyecto compile en clones donde ese
    // folder no existe; si el seed no está presente simplemente no hace nada.
    public static class DevSeedBootstrap
    {
        public static async Task RunAsync(IServiceProvider serviceProvider)
        {
            var seedType = Type.GetType("RealEstateApp.Presentation.WebApp.Seeds.DevDataSeed");
            var method = seedType?.GetMethod("SeedAsync");
            if (method?.Invoke(null, new object[] { serviceProvider }) is Task task)
            {
                await task;
            }
        }
    }
}
