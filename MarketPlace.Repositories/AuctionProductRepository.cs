using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class AuctionProductRepository : BaseRepository<AuctionProduct>, IAuctionProductRepository
    {
        internal AuctionProductRepository(DbContext context) : base(context) { }
    }
}
