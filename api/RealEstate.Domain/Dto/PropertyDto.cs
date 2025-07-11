namespace RealEstate.Domain.Dto
{
    public class PropertyDto
    {
        public string? Id { get; set; }
        public required string IdOwner { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public decimal Price { get; set; }
        public required string ImageUrl { get; set; }

        public OwnerDto? Owner { get; set; }
    }
}
