using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMAPI.Models.Query;
using PMCore.Configuration;
using PMDB.Models;

namespace PMAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OptionController : BaseController
	{
		private readonly PmdbContext _context;
		private readonly IMapper _mapper;

		public OptionController(PmdbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet(nameof(AuthCompanyList) + "/{value?}")]
		[Authorize(Roles = AppConst.Role.Company)]
		public async Task<ActionResult<List<OptionModel>>> AuthCompanyList(string? value = null)
		{
			var rootDept = await _context.TbOrgRoleUsers.Where(x => x.Uid == _uid && x.Rid == AppConst.Role.Organization).Select(x => x.RootDid).ToListAsync();
			var query = _context.VwOrgCompanies.Where(x => rootDept.Contains(x.Did));

			if (!string.IsNullOrWhiteSpace(value))
			{
				query = query.Where(x => x.DeptName.Contains(value));
			}

			return query.Select(x => new OptionModel { Value = x.Did, Label = x.DeptName }).OrderBy(x => x.Label).ToList();
		}


	}
}
