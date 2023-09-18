using GenericRepository.Repository;
using Microsoft.EntityFrameworkCore;
using PatternRepository.Models.DB;

namespace PatternRepository.Repository.UserRepository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private ApplicationContext _context;
        public UserRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
