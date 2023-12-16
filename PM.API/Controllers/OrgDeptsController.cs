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


		[HttpPost(nameof(Company))]
		[Authorize(Roles = $"{AppConst.Role.Company}")]
		public async Task<ActionResult> Company(CompanyModel model)
		{
			// Add
			if (string.IsNullOrWhiteSpace(model.Did) || !TbOrgDeptExists(model.Did))
			{
				var dept = _mapper.Map<TbOrgDept>(model);
				dept.Did = Guid.NewGuid().ToString().Replace("-", "");
				dept.RootDid = dept.Did;
				dept.Enable = true;
				dept.Sort = _context.TbOrgDepts.Select(x => x.Sort).OrderByDescending(x => x).FirstOrDefaultAsync().Result;
				dept.Expand = false;
				_context.TbOrgDepts.Add(dept);
				_context.TbOrgRoleUsers.Add(
					new TbOrgRoleUser
					{
						Rid = AppConst.Role.Organization,
						Uid = _uid,
						RootDid = dept.Did
					});

			}
			else
			{
				var targetDept = await _context.TbOrgDepts.FirstOrDefaultAsync(x => x.Did == model.Did);

				if (targetDept == null)
				{
					return NotFound();
				}

				_context.Entry(targetDept).CurrentValues.SetValues(model);
			}


			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateException)
			{
				if (!TbOrgDeptExists(model.Did))
				{
					//return Conflict();
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
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
		private bool TbOrgDeptExists(string id)
		{
			return (_context.TbOrgDepts?.Any(e => e.Did == id)).GetValueOrDefault();
		}
	}
}
