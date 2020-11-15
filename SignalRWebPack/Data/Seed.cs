using SignalRWebPack.Data.EntityRepositories;
using SignalRWebPack.Models;
using System.Collections.Generic;

namespace SignalRWebPack.Data
{
    public class Seed
    {
        private readonly UserRepository userRepository;

        public Seed(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void Execute()
        {
            List<User> users = new List<User>
            {
                new User
                {
                    Name = "Lui",
                    Password = "Lui123"
                },
                new User
                {
                    Name = "Laura",
                    Password = "Laura123"
                },
                new User
                {
                    Name = "Ioni",
                    Password = "Ioni123"
                },
                new User
                {
                    Name = "Dídio",
                    Password = "Didio123"
                },
                new User
                {
                    Name = "João",
                    Password = "Joao123"
                },
                new User
                {
                    Name = "Warte",
                    Password = "Warte123"
                }
            };

            userRepository.Create(users);
            userRepository.Commit();
        }
    }
}
