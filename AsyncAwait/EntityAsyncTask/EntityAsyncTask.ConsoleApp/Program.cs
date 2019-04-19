using EntityAsyncTask.Core;
using System;
using System.Threading.Tasks;

namespace EntityAsyncTask.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var usersRepository = new UserRepository();

            var newUser = new User { Id = 1, FirstName = "UserFirstName", LastName = "UserLastName", Age = 30 };
            await usersRepository.AddAsync(newUser);
            Console.WriteLine(await usersRepository.GetByIdAsync(1));

            try
            {
                var user = await usersRepository.GetByIdAsync(2);
            }
            catch(RequestedResourceNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            newUser.Age = 50;
            Console.WriteLine(await usersRepository.UpdateAsync(newUser));

            try
            {
                await usersRepository.UpdateAsync(new User { Id = 100 });
            }
            catch(RequestedResourceNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            try
            {
                await usersRepository.AddAsync(new User { Id = 1 });
            }
            catch(RequestedResourceHasConflictException e)
            {
                Console.WriteLine(e.Message);
            }

            await usersRepository.DeleteAsync(1);
            Console.WriteLine("User with id 1 is deleted.");

            try
            {
                await usersRepository.DeleteAsync(1);
            }
            catch(RequestedResourceNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
