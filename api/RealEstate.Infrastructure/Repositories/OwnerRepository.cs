using MongoDB.Driver;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IMongoCollection<Owner> _owners;

        public OwnerRepository(IMongoDatabase database)
        {
            _owners = database.GetCollection<Owner>("owners");
        }

        public async Task<PaginatedList<Owner>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var total = (int)await _owners.CountDocumentsAsync(_ => true);
            var owners = await _owners
                .Find(_ => true)
                .Skip((pageNumber - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PaginatedList<Owner>
            {
                Items = owners,
                TotalCount = total
            };
        }


        public async Task<Owner?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _owners.Find(o => o.Id == id).FirstOrDefaultAsync();
        }

        public Task CreateAsync(Owner owner) =>
            _owners.InsertOneAsync(owner);

        public Task UpdateAsync(Owner owner) =>
            _owners.ReplaceOneAsync(x => x.Id == owner.Id, owner);

        public Task DeleteAsync(string id) =>
            _owners.DeleteOneAsync(o => o.Id == id);
    }
}
