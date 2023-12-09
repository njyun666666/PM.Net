using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMAPI.Errors;
using PMAPI.Models.OrgDepts;
using PMCore.Configuration;
using PMDB.Models;
using System.Net;

namespace PMAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = $"{AppConst.Role.Organization},{AppConst.Role.Company}")]
	public class OrgDeptsController : BaseController
	{
		private readonly PmdbContext _context;
		private readonly IMapper _mapper;

		public OrgDeptsController(PmdbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		[HttpGet(nameof(CompanyList))]
		public async Task<ActionResult<List<CompanyViewModel>>> CompanyList()
		{
			var rootDept = await _context.TbOrgRoleUsers.Where(x => x.Uid == _uid && x.Rid == AppConst.Role.Organization).Select(x => x.RootDid).ToListAsync();
			var listQuery = _context.VwOrgCompanies.Where(x => rootDept.Contains(x.RootDid)).OrderBy(x => x.DeptName);
			return _mapper.Map<List<CompanyViewModel>>(listQuery);
		}

		// POST: api/OrgDepts
		[HttpPost]
		public async Task<ActionResult<List<OrgDeptsViewModel>>> GetTbOrgDepts(OrgDeptsModel model)
		{
			var isAuthRootDept = await _context.TbOrgRoleUsers.Where(x => x.Uid == _uid && x.Rid == AppConst.Role.Organization && x.RootDid == model.Did).AnyAsync();

			if (!isAuthRootDept)
			{
				throw new RestException(HttpStatusCode.Forbidden);
			}

			var listQuery = _context.VwOrgDepts.Where(x => x.RootDid == model.Did).OrderBy(x => x.DeptName);

			return _mapper.Map<List<OrgDeptsViewModel>>(listQuery);
		}

	}
}
