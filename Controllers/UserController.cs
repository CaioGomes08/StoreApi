using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Repositories;
using ProductCatalog.ViewModels.UserViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

       
    }
}
