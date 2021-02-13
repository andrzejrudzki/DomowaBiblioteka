using DomowaBiblioteka.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DomowaBiblioteka.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]

        public async Task<IQueryable<IdentityUser>> Get()
        {
            return await Task.Run(() => _userManager.Users);
        }

        [HttpGet("{id}")]

        public async Task<IdentityUser> Get(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterViewModel model)
        {
            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] IdentityUser userToUpdate)
        {
            var user = await _userManager.FindByIdAsync(id);

            user.Email = userToUpdate.Email;
            user.UserName = userToUpdate.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] IdentityUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }
    }
}
