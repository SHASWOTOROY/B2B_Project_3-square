using Threesquare.Core.Repositories;

namespace MarketPlace.Services
{
    public class OrderService
    {
        public OrderService(IRepositorySession repositorySession) { _RepositorySession = repositorySession; }

        private IRepositorySession _RepositorySession { get; set; }
    }
}
