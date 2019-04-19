using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityAsyncTask.Core
{
    public class UserRepository : IRepository<User>
    {
        private List<User> Users { get; } = new List<User>();

        public async Task<User> GetByIdAsync(int id)
        {
            var result = await Task.Run(async() =>
            {
                await Task.Delay(500);

                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    throw new RequestedResourceNotFoundException($"No user with id {id} is founded.");
                }

                return user;
            });

            return result;
        }

        public async Task<User> AddAsync(User user)
        {
            var result = await Task.Run(async () =>
            {
                await Task.Delay(500);

                if (Users.FirstOrDefault(u => u.Id == user.Id) != null)
                {
                    throw new RequestedResourceHasConflictException($"User with id {user.Id} already exists.");
                }

                Users.Add(user);

                return user;
            });

            return result;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var result = await Task.Run(async () =>
            {
                await Task.Delay(500);

                if (Users.FirstOrDefault(u => u.Id == user.Id) == null)
                {
                    throw new RequestedResourceNotFoundException($"No user with id {user.Id} is founded.");
                }

                int userIndex = Users.FindIndex(u => u.Id == user.Id);
                Users[userIndex] = user;

                return user;
            });

            return user;
        }

        public async Task DeleteAsync(int userId)
        {
            await Task.Run(async () =>
            {
                await Task.Delay(500);

                var user = Users.FirstOrDefault(u => u.Id == userId);
                if (user == null)
                {
                    throw new RequestedResourceNotFoundException($"No user with id {userId} is founded.");
                }

                Users.Remove(user);
            });
        }
    }
}
