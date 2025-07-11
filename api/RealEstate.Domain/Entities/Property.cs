namespace RealEstate.Domain.Entities
{
    public class Property
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public required string IdOwner { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public decimal Price { get; set; }
        public required string ImageUrl { get; set; }
    }
}


