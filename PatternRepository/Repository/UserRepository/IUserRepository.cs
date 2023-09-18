using GenericRepository.Repository.Interfaces;
using PatternRepository.Models.DB;

namespace PatternRepository.Repository.UserRepository
{
    public interface IUserRepository: IGenericRepository<User>
    {
    }
}
