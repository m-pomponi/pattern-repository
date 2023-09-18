using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PatternRepository.Models.DB;
using PatternRepository.Repository.UserRepository;

namespace PatternRepository.Controllers
{
    [Route("user")]
    [ApiController]
    public class UserController : Controller
    {
        private IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _repository.GetAllAsync();

            return Ok(response);
        }

        [HttpGet("{idUser}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int idUser)
        {
            var response = await _repository.GetByIdAsync(idUser);

            if (response == null)
                return BadRequest("User not found");

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AddUser([FromBody] User request)
        {
            var response = await _repository.AddAsync(request);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _repository.GetByIdAsync(id);

            if(user == null)
                return BadRequest("User not found");

            var response = await _repository.DeleteAsync(user);
            return Ok(response);

        }
    }
}
