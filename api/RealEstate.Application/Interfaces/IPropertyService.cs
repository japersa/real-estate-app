using RealEstate.Domain.Dto;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<PaginatedList<PropertyDto>> GetFilteredPagedAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);
        Task<PropertyDto?> GetByIdAsync(string id);
        Task CreateAsync(PropertyDto property);
        Task UpdateAsync(string id, PropertyDto property);
        Task DeleteAsync(string id);
    }
}
