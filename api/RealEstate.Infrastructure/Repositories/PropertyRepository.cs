using MongoDB.Bson;
using MongoDB.Driver;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyRepository : IPropertyRepository
    {
        private readonly IMongoCollection<Property> _collection;

        public PropertyRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Property>("properties");
        }
        public async Task<(List<Property>, int)> GetFilteredPagedAsync(string? name, string? address, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var builder = Builders<Property>.Filter;
            var filters = new List<FilterDefinition<Property>>();

            if (!string.IsNullOrWhiteSpace(name))
                filters.Add(builder.Regex("Name", new BsonRegularExpression(name, "i")));
            if (!string.IsNullOrWhiteSpace(address))
                filters.Add(builder.Regex("Address", new BsonRegularExpression(address, "i")));
            if (minPrice.HasValue)
                filters.Add(builder.Gte("Price", minPrice.Value));
            if (maxPrice.HasValue)
                filters.Add(builder.Lte("Price", maxPrice.Value));

            var filter = filters.Any() ? builder.And(filters) : builder.Empty;

            var total = await _collection.CountDocumentsAsync(filter);
            var items = await _collection.Find(filter)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Limit(pageSize)
                                        .ToListAsync();

            return (items, (int)total);
        }

        public async Task<Property?> GetByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        
        public Task CreateAsync(Property p) => _collection.InsertOneAsync(p);
        public Task UpdateAsync(Property p) => _collection.ReplaceOneAsync(x => x.Id == p.Id, p);
        public Task DeleteAsync(string id) => _collection.DeleteOneAsync(p => p.Id == id);
    }
}
