using MongoDB.Driver;
using RealEstate.Domain.Entities;
using RealEstate.Application.Interfaces;

namespace RealEstate.Infrastructure.Repositories 
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoDatabase database)
        {
            _users = database.GetCollection<User>("users");
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
        
        public Task CreateAsync(User user) =>
            _users.InsertOneAsync(user);
    }
}
