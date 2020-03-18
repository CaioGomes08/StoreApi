using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductCatalog.Helpers;
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

        private readonly IConfiguration _configuration;

        public UserController(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
                var senha = Encrypt.EncryptPassword(user.Senha);

                var userResult = _userRepository.GetUserByEmailAndPassword(user.Email, senha);

                if (userResult != null)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecrectKey")));
                    var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userResult.Nome),
                        new Claim(ClaimTypes.Role, userResult.Perfil)
                    };

                    var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:5001", // nome do servidor web que emite o token
                        audience: "https://localhost:5001", // destinatários validos
                        claims: claims, // define as permissoes dos usuarios se é Administrador
                        expires: DateTime.Now.AddMinutes(10), // em quanto tempo o token expira
                        signingCredentials: signinCredentials
                        );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                    return Ok(new { Token = tokenString, Usuario = userResult });
                }
                else
                {
                    return Unauthorized();
                }
            }

           
        }

    }
}
