using RealEstate.Application.Interfaces;
using RealEstate.Domain.Dto;
using RealEstate.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Application.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IOwnerRepository _repo;

        public OwnerService(IOwnerRepository repo)
        {
            _repo = repo;
        }

        public async Task<PaginatedList<OwnerDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var pagedOwners = await _repo.GetPagedAsync(pageNumber, pageSize);
            var dtoList = pagedOwners.Items.Select(o => new OwnerDto
            {
                Id = o.Id,
                Name = o.Name,
                Email = o.Email
            }).ToList();

            return new PaginatedList<OwnerDto>
            {
                Items = dtoList,
                TotalCount = pagedOwners.TotalCount
            };
        }

        public async Task<OwnerDto?> GetByIdAsync(string id)
        {
            var owner = await _repo.GetByIdAsync(id);
            if (owner == null) return null;
            return new OwnerDto
            {
                Id = owner.Id,
                Name = owner.Name,
                Email = owner.Email
            };
        }

        public async Task CreateAsync(OwnerDto dto)
        {
            var owner = new Owner
            {
                Name = dto.Name,
                Email = dto.Email
            };
            await _repo.CreateAsync(owner);
        }

        public async Task UpdateAsync(string id, OwnerDto dto)
        {
            var owner = await _repo.GetByIdAsync(id);
            if (owner == null) throw new Exception("Owner no encontrado");
            owner.Name = dto.Name;
            owner.Email = dto.Email;
            await _repo.UpdateAsync(owner);
        }

        public Task DeleteAsync(string id) => _repo.DeleteAsync(id);
    }
}
