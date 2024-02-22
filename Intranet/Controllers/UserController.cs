using AutoMapper;
using Intranet.Models;
using Intranet.Models.PageItems;
using Intranet.Models.PageItems.Dto;
using Intranet.Repository.Entities;
using Intranet.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Query = Intranet.Repository.Entities.Query;

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

            UserEntity userEntity = await _userRepository.GetBySid(id);

            UserViewModel userViewModel = _mapper.Map<UserEntity, UserViewModel>(userEntity) ?? new UserViewModel();

            return View(userViewModel); 
        }

        public async Task<IActionResult> ViewUsers(ItemsDto itemsDto)
        {
            PageResult<UserEntity> users = await _userRepository.GetPage(_mapper.Map<ItemsDto, Query>(itemsDto));

            PageItemsViewModel<UserViewModel> usersViewModel = _mapper.Map<PageResult<UserEntity>, PageItemsViewModel<UserViewModel>>(users);

            usersViewModel.PageSize = itemsDto.Page.PageSize;

            return View(usersViewModel);
        }
    }
}
