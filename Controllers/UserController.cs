using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Models;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Controllers
{
    [Route("api/v1/[controller]")]
    public class UserController : Controller
    {

        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpGet]
        public IEnumerable<ListUserViewModel> GetUser()
        {
            return _userRepository.Get();
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid client request");
            }
            else
            {
                var userResult = _userRepository.GetUserByEmailAndPassword(user.Email, user.Senha);

                if (userResult != null)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Nome),
                        new Claim(ClaimTypes.Role, "administrador")
                    };

                    var tokenOptions = new JwtSecurityToken(
                        issuer: "http://localhost:5000", // nome do servidor web que emite o token
                        audience: "http://localhost:5000", // destinatários validos
                        claims: claims, // define as permissoes dos usuarios se é Administrador
                        expires: DateTime.Now.AddMinutes(10), // em quanto tempo o token expira
                        signingCredentials: signinCredentials
                        );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    return Ok(new { Token = tokenString });
                }
                else
                {
                    return Unauthorized();
                }
            }

           
        }

    }
}
