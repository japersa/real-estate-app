namespace RealEstate.Domain.Entities
{
    public class Owner
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
