using AutoMapper;
using Intranet.Models;
using Intranet.Repository.Entities;
using Intranet.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Intranet.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IActionResult> ViewUser()
        {
            UserEntity userEntity = await _userRepository.GetById(User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid").Value);

            UserViewModel userViewModel = _mapper.Map<UserEntity, UserViewModel>(userEntity);

            return View(userViewModel); 
        }

        public IActionResult ViewUsers()
        {
            return View();
        }
    }
}
