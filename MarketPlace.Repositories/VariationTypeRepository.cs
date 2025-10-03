using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class VariationTypeRepository : BaseRepository<VariationType>, IVariationTypeRepository
    {
        internal VariationTypeRepository(DbContext context) : base(context) { }
    }
}
