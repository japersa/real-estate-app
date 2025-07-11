using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<(List<Property>, int)> GetFilteredPagedAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);
        Task<Property?> GetByIdAsync(string id);
        Task CreateAsync(Property property);
        Task UpdateAsync(Property property);
        Task DeleteAsync(string id);
    }
}
