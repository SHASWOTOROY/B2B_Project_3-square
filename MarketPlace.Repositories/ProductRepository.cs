using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        internal ProductRepository(DbContext context) : base(context) { }
    }
}
