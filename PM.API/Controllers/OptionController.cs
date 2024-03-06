using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMAPI.Errors;
using PMAPI.Models.Query;
using PMAPI.Services.IServices;
using PMCore.Configuration;
using PMDB.Models;
using System.Net;

namespace PMAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class OptionController : BaseController
	{
		private readonly PmdbContext _context;
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;

		public OptionController(PmdbContext context, IMapper mapper, IAuthService authService)
		{
			_context = context;
			_mapper = mapper;
			_authService = authService;
		}

		[HttpGet(nameof(AuthCompanyList) + "/{value?}")]
		[Authorize(Roles = $"{AppConst.Role.Administrator},{AppConst.Role.Company}")]
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

		[HttpGet(nameof(AuthCompanyUserList) + "/{company}")]
		[Authorize(Roles = $"{AppConst.Role.Administrator},{AppConst.Role.Company}")]
		public async Task<ActionResult<List<OptionModel>>> AuthCompanyUserList([FromQuery] OptionQueryModel model, string company)
		{
			if (!await _authService.CheckOrgAdmin(company, _uid))
			{
				throw new RestException(HttpStatusCode.Forbidden);
			}

			var query = _context.VwOrgUsers.Where(x => x.RootDid == company);

			if (!string.IsNullOrWhiteSpace(model.Input))
			{
				query = query.Where(x => x.Name.Contains(model.Input));
			}

			return query.Select(x => new OptionModel { Value = x.Uid, Label = x.Name }).Distinct().OrderBy(x => x.Label).ToList();
		}

	}
}
