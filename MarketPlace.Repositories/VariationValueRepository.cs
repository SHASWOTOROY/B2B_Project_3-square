using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class VariationValueRepository : BaseRepository<VariationValue>, IVariationValueRepository
    {
        internal VariationValueRepository(DbContext context) : base(context) { }
    }
}
