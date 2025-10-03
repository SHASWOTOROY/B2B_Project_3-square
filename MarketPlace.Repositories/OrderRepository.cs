using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        internal OrderRepository(DbContext context) : base(context) { }
    }
}
