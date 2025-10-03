using Threesquare.Core.Repositories;

namespace MarketPlace.Services
{
    public class ProductService
    {
        public ProductService(IRepositorySession repositorySession) { _RepositorySession = repositorySession; }

        private IRepositorySession _RepositorySession { get; set; }
    }
}
