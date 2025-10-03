using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class AuctionRepository : BaseRepository<Auction>, IAuctionRepository
    {
        internal AuctionRepository(DbContext context) : base(context) { }
    }
}
