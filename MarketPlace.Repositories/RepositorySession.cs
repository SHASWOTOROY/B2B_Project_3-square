using MarketPlace.Services.Repositories;
using Threesquare.Core.Repositories;
using Threesquare.Core.Repositories.EntityFramework;

namespace MarketPlace.Repositories
{
    public class RepositorySession : BaseRepositorySession, IRepositorySession
    {
        public RepositorySession(BaseDatabaseContext context) : base(context) { _Context = context; }

        public IUserRepository GetUserRepository() => new UserRepository(_Context);
        public ICompanyRepository GetCompanyRepository() => new CompanyRepository(_Context);

        public IBrandRepository GetBrandRepository() => new BrandRepository(_Context);
        public ICategoryRepository GetCategoryRepository() => new CategoryRepository(_Context);
        public IVariationTypeRepository GetVariationTypeRepository() => new VariationTypeRepository(_Context);
        public IVariationValueRepository GetVariationValueRepository() => new VariationValueRepository(_Context);
        public IProductRepository GetProductRepository() => new ProductRepository(_Context);

        public IAuctionRepository GetAuctionRepository() => new AuctionRepository(_Context);
        public IAuctionProductRepository GetAuctionProduct() => new AuctionProductRepository(_Context);
        public IBidRepository GetBidRepository() => new BidRepository(_Context);

        public IOrderRepository GetOrderRepository() => new OrderRepository(_Context);
        public IOrderProductRepository GetOrderProductRepository    () => new OrderProductRepository(_Context);

        private BaseDatabaseContext _Context;
    }
}
