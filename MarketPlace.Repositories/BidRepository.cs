using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class BidRepository : BaseRepository<Bid>, IBidRepository
    {
        internal BidRepository(DbContext context) : base(context) { }
    }
}
