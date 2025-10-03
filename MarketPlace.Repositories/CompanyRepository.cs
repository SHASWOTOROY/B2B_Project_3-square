using MarketPlace.Models;
using MarketPlace.Services.Repositories;
using Microsoft.EntityFrameworkCore;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        internal CompanyRepository(DbContext context) : base(context) { }
    }
}
