using Threesquare.Core.Repositories;

namespace MarketPlace.Services.Repositories
{
    public interface IMarketPlaceRepositorySession : IRepositorySession, IDisposable
    {
        IUserRepository GetUserRepository();
        ICompanyRepository GetCompanyRepository();

        IBrandRepository GetBrandRepository();
        ICategoryRepository GetCategoryRepository();
        IVariationTypeRepository GetVariationTypeRepository();
        IVariationValueRepository GetVariationValueRepository();
        IProductRepository GetProductRepository();

        IAuctionRepository GetAuctionRepository();
        IAuctionProductRepository GetAuctionProduct();
        IBidRepository GetBidRepository();

        IOrderRepository GetOrderRepository();
        IOrderProductRepository GetOrderProductRepository();
    }
}
