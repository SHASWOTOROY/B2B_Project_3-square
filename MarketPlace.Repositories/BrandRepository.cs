using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class BrandRepository : BaseRepository<Brand>, IBrandRepository
    {
        internal BrandRepository(DbContext context) : base(context) { }
    }
}
