using RealEstate.Application.Interfaces;
using RealEstate.Domain.Dto;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _repo;
        private readonly IOwnerRepository _ownerRepo;

        public PropertyService(IPropertyRepository repo, IOwnerRepository ownerRepo)
        {
            _repo = repo;
            _ownerRepo = ownerRepo;
        }

        public async Task<PaginatedList<PropertyDto>> GetFilteredPagedAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var (properties, totalCount) = await _repo.GetFilteredPagedAsync(name, address, minPrice, maxPrice, pageNumber, pageSize);

            var dtos = new List<PropertyDto>();

            foreach (var prop in properties)
            {
                var owner = await _ownerRepo.GetByIdAsync(prop.IdOwner);
                dtos.Add(new PropertyDto
                {
                    Id = prop.Id,
                    IdOwner = prop.IdOwner,
                    Name = prop.Name,
                    Address = prop.Address,
                    Price = prop.Price,
                    ImageUrl = prop.ImageUrl,
                    Owner = owner == null ? null : new OwnerDto
                    {
                        Id = owner.Id,
                        Name = owner.Name,
                        Email = owner.Email,
                    }
                });
            }

            return new PaginatedList<PropertyDto>
            {
                Items = dtos,
                TotalCount = totalCount
            };
        }

        public async Task<PropertyDto?> GetByIdAsync(string id)
        {
            var prop = await _repo.GetByIdAsync(id);
            if (prop == null) return null;

            var owner = await _ownerRepo.GetByIdAsync(prop.IdOwner);

            return new PropertyDto
            {
                Id = prop.Id,
                IdOwner = prop.IdOwner,
                Name = prop.Name,
                Address = prop.Address,
                Price = prop.Price,
                ImageUrl = prop.ImageUrl,
                Owner = owner == null ? null : new OwnerDto
                {
                    Id = owner.Id,
                    Name = owner.Name,
                    Email = owner.Email
                }
            };
        }

        public Task CreateAsync(PropertyDto dto)
        {
            var entity = new Property
            {
                Id = Guid.NewGuid().ToString(),
                IdOwner = dto.IdOwner,
                Name = dto.Name,
                Address = dto.Address,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl
            };

            dto.Id = entity.Id;

            return _repo.CreateAsync(entity);
        }

        public async Task UpdateAsync(string id, PropertyDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                throw new Exception("Property not found");

            existing.Name = dto.Name;
            existing.Address = dto.Address;
            existing.Price = dto.Price;
            existing.ImageUrl = dto.ImageUrl;
            existing.IdOwner = dto.IdOwner;

            await _repo.UpdateAsync(existing);
        }

        public Task DeleteAsync(string id) => _repo.DeleteAsync(id);
    }
}
