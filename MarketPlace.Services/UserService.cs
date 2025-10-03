using Threesquare.Core.Repositories;

namespace MarketPlace.Services
{
    public class UserService
    {
        public UserService(IRepositorySession repositorySession) { _RepositorySession = repositorySession; }

        private IRepositorySession _RepositorySession { get; set; }
    }
}
