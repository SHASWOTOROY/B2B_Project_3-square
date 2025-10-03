using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        internal UserRepository(DbContext context) : base(context) { }
    }
}
