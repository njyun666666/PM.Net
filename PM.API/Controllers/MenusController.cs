using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMAPI.Models.Menu;
using PMDB.Models;
using System.Security.Claims;

namespace PMAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class MenusController : BaseController
	{
		private readonly PmdbContext _context;
		private readonly IMapper _mapper;
		List<TbMenu> _menus = null!;

		public MenusController(PmdbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// GET: api/Menus
		[HttpGet]
		public async Task<ActionResult<List<MenuViewModel>>> GetTbMenus()
		{
			var roles = GetUserClaim(ClaimTypes.Role);

			_menus = await _context.TbMenus
				.Where(m => m.Enable && m.Rids.Where(r => r.Rid == "everyone" || roles.Contains(r.Rid)).Any())
				.OrderBy(m => m.Sort).ToListAsync();

			return SetMenu(null);
		}

		private List<MenuViewModel> SetMenu(string? parentMenuID)
		{
			IEnumerable<TbMenu> menuList;

			if (string.IsNullOrWhiteSpace(parentMenuID))
			{
				menuList = _menus.Where(x => x.ParentMenuId == null || x.ParentMenuId == "");
			}
			else
			{
				menuList = _menus.Where(x => x.ParentMenuId == parentMenuID);
			}

			if (!menuList.Any())
			{
				return null;
			}

			var menus = _mapper.Map<List<MenuViewModel>>(menuList);

			menus.ForEach(m =>
			{
				m.Children = SetMenu(m.MenuId);
			});

			return menus;
		}
	}
}
