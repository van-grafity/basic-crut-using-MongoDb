using crud.repository.models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace crud.repository.services
{
    

    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(string config)
        {
            var client = new MongoClient(config);
            var database = client.GetDatabase("crud");
            _users = database.GetCollection<User>("Users");
        }
        public async Task<User> Insert(User user)
        {
            user.Created = DateTime.Now;
            await _users.InsertOneAsync(user).ConfigureAwait(false);
            return user;
        }

        public async Task<User> Find(string email)
        {
            var user =  await _users.FindAsync<User>(u => u.Email == email).ConfigureAwait(false);
            return await user.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task Update(User user)
        {
            //var filter = Builders<User>.Filter.Eq(u => u.Id, user.Id);
            var update = Builders<User>.Update
                                       .Set(x => x.FirstName, user.FirstName)
                                       .Set(x => x.LastName, user.LastName)
                                       .Set(x => x.Email, user.Email);

            await _users.UpdateOneAsync<User>( u => u.Id == user.Id, update).ConfigureAwait(false);
        }

        public async Task Delete(Guid Id)
        {
            await _users.DeleteOneAsync(u => u.Id == Id).ConfigureAwait(false);
        }
    }
}
