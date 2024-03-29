﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMAPI.Errors;
using PMAPI.Helper;
using PMAPI.Models.OrgDepts;
using PMAPI.Models.Query;
using PMCore.Configuration;
using PMCore.Helpers;
using PMDB.Models;
using System.Linq.Dynamic.Core;
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

		[HttpPost(nameof(QueryCompany))]
		[Authorize(Roles = $"{AppConst.Role.Company}")]
		public async Task<ActionResult<QueryViewModel<List<CompanyViewModel>>>> QueryCompany(QueryModel<CompanyModel> model)
		{
			var listQuery = _context.VwOrgCompanies.AsQueryable();

			if (model.Filter != null && !string.IsNullOrWhiteSpace(model.Filter.DeptName))
			{
				listQuery = listQuery.Where(x => x.DeptName.Contains(model.Filter.DeptName));
			}

			var count = listQuery.Count();

			if (string.IsNullOrWhiteSpace(model.Sort))
			{
				listQuery = listQuery.OrderBy(x => x.DeptName);
			}
			else
			{
				listQuery = listQuery.OrderBy(model.OrderBy);
			}

			listQuery = listQuery.Skip(model.Skip).Take(model.PageSize);

			return new QueryViewModel<List<CompanyViewModel>>
			{
				Data = await _mapper.ProjectTo<CompanyViewModel>(listQuery).ToListAsync(),
				PageCount = model.PageCount(count)
			};
		}

		[HttpPost(nameof(Company))]
		[Authorize(Roles = $"{AppConst.Role.Company}")]
		public async Task<ActionResult> Company(CompanyModel model)
		{
			// Add
			if (string.IsNullOrWhiteSpace(model.Did) || !TbOrgDeptExists(model.Did))
			{
				var dept = _mapper.Map<TbOrgDept>(model);
				dept.Did = EncodingHepler.NewID();
				dept.RootDid = dept.Did;
				dept.Enable = true;
				dept.Sort = _context.TbOrgDepts.Select(x => x.Sort).OrderByDescending(x => x).FirstOrDefaultAsync().Result + 1;
				dept.Expand = false;
				await _context.TbOrgDepts.AddAsync(dept);
				await _context.TbOrgRoleUsers.AddAsync(
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
				await _context.SaveChangesWithLogAsync(_uid);
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

			return await _mapper.ProjectTo<OrgDeptsViewModel>(listQuery).ToListAsync();
		}
		private bool TbOrgDeptExists(string id)
		{
			return (_context.TbOrgDepts?.Any(e => e.Did == id)).GetValueOrDefault();
		}
	}
}
