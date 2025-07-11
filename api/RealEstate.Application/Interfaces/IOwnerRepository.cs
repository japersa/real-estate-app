using RealEstate.Domain.Entities;

namespace RealEstate.Application.Interfaces
{
    public interface IOwnerRepository
    {
        Task<PaginatedList<Owner>> GetPagedAsync(int pageNumber, int pageSize);
        Task<Owner?> GetByIdAsync(string id);
        Task CreateAsync(Owner owner);
        Task UpdateAsync(Owner owner);
        Task DeleteAsync(string id);
    }
}
