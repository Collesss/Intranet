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

        public async Task<IActionResult> ViewUser(string id)
        {
            id = string.IsNullOrEmpty(id) ? User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/primarysid").Value : id;

            UserEntity userEntity = await _userRepository.GetById(id);

            UserViewModel userViewModel = _mapper.Map<UserEntity, UserViewModel>(userEntity);

            return View(userViewModel); 
        }

        public async Task<IActionResult> ViewUsers()
        {
            IEnumerable<UserEntity> usersEntity = await _userRepository.GetAll();

            IEnumerable<UserViewModel> usersViewModel = _mapper.Map<IEnumerable<UserEntity>, IEnumerable<UserViewModel>>(usersEntity);

            return View(usersViewModel);
        }
    }
}
