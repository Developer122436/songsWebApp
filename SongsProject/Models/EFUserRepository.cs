using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsProject.Models
{
        public class EFUserRepository : IUserRepository
        {
            private ApplicationDbContext context;

            public EFUserRepository(ApplicationDbContext ctx)
            {
                context = ctx;
            }

            public IQueryable<User> Users => context.Users;

            public void SaveUser(User user)
            {
                if (user.Id == 0)
                {
                    context.Users.Add(user);
                }
                else
                {
                    User dbEntry = context.Users
                    .FirstOrDefault(p => p.Id == user.Id);
                    if (dbEntry != null)
                    {
                        dbEntry.Name = user.Name;
                        dbEntry.Password = user.Password; 
                    }
                }
                context.SaveChanges();
            }
        }
}


