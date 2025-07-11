using RealEstate.Domain.Dto;

namespace RealEstate.Application.Interfaces
{
    public interface IOwnerService
    {
        Task<PaginatedList<OwnerDto>> GetPagedAsync(int pageNumber, int pageSize);
        Task<OwnerDto?> GetByIdAsync(string id);
        Task CreateAsync(OwnerDto ownerDto);
        Task UpdateAsync(string id, OwnerDto ownerDto);
        Task DeleteAsync(string id);
    }
}
