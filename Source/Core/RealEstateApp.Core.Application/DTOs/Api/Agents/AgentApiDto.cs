namespace RealEstateApp.Core.Application.DTOs.Api.Agents
{
    public sealed class AgentApiDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public int NumberOfProperties { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required bool IsActive { get; set; }
    }
}
