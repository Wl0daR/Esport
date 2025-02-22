using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Esport.Shared.DTO; // Wspólne modele DTO
using System.Linq;
using System.Threading.Tasks;

namespace Esport.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Używamy pola Login jako UserName, a Email jako Email
            var user = new IdentityUser
            {
                UserName = model.Login,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Przypisujemy domyślną rolę "Użytkownik"
                await _userManager.AddToRoleAsync(user, "Użytkownik");
                return Ok("Rejestracja zakończona powodzeniem.");
            }
            return BadRequest(result.Errors);
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Próba znalezienia użytkownika po emailu, a potem po nazwie użytkownika
            IdentityUser? user = await _userManager.FindByEmailAsync(model.Identifier)
                ?? await _userManager.FindByNameAsync(model.Identifier);

            if (user == null)
                return Unauthorized("Niepoprawne dane logowania.");

            var result = await _signInManager.PasswordSignInAsync(user.UserName!, model.Password, false, false);
            if (result.Succeeded)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "Brak roli";
                var loginResult = new LoginResult
                {
                    UserName = user.UserName,
                    Role = role
                };
                return Ok(loginResult);
            }
            return Unauthorized("Niepoprawne dane logowania.");
        }

        // POST: api/account/logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Wylogowano pomyślnie.");
        }
    }
}
