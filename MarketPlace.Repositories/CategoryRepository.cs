using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        internal CategoryRepository(DbContext context) : base(context) { }
    }
}
