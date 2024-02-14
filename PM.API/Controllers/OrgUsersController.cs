using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PMAPI.Errors;
using PMAPI.Helper;
using PMAPI.Models.OrgUser;
using PMAPI.Models.Query;
using PMAPI.Services.IServices;
using PMCore.Configuration;
using PMCore.Helpers;
using PMDB.Models;
using System.Linq.Dynamic.Core;
using System.Net;

namespace PMAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = AppConst.Role.Organization)]
	public class OrgUsersController : BaseController
	{
		private readonly PmdbContext _context;
		private readonly IMapper _mapper;
		private readonly IAuthService _authService;

		public OrgUsersController(PmdbContext context, IMapper mapper, IAuthService authService)
		{
			_context = context;
			_mapper = mapper;
			_authService = authService;
		}

		// POST: api/OrgUsers/QueryUsers
		[HttpPost(nameof(QueryUsers))]
		public async Task<ActionResult<QueryViewModel<List<OrgUserViewModel>>>> QueryUsers(QueryModel<OrgUserQueryModel> model)
		{
			if (model.Filter == null || !await _authService.CheckOrgAdmin(model.Filter.RootDID, _uid))
			{
				throw new RestException(HttpStatusCode.Forbidden);
			}

			var listQuery = _context.TbOrgUsers.Where(x => x.TbOrgDeptUsers.Any(du => du.DidNavigation.RootDid == model.Filter.RootDID));

			if (model.Filter != null)
			{
				if (!string.IsNullOrWhiteSpace(model.Filter.Name))
				{
					listQuery = listQuery.Where(x => x.Name.Contains(model.Filter.Name));
				}

				if (!string.IsNullOrWhiteSpace(model.Filter.EMail))
				{
					listQuery = listQuery.Where(x => x.Email.Contains(model.Filter.EMail));
				}
			}

			var count = listQuery.Count();

			if (string.IsNullOrWhiteSpace(model.Sort))
			{
				listQuery = listQuery.OrderBy(x => x.Name);
			}
			else
			{
				listQuery = listQuery.OrderBy(model.OrderBy);
			}

			listQuery = listQuery.Skip(model.Skip).Take(model.PageSize);

			return new QueryViewModel<List<OrgUserViewModel>>
			{
				Data = await _mapper.ProjectTo<OrgUserViewModel>(listQuery).ToListAsync(),
				PageCount = model.PageCount(count)
			};
		}

		// GET: api/OrgUsers/Depts/{id}
		[HttpGet(nameof(Depts) + "/{id}")]
		public async Task<ActionResult<List<OrgUserDeptModel>>> Depts(string id)
		{
			var listQuery = _context.VwOrgCompanyUsers.Where(x => x.Uid == id);
			return await _mapper.ProjectTo<OrgUserDeptModel>(listQuery).ToListAsync();
		}

		[HttpPost]
		public async Task<ActionResult> OrgUser(OrgUserModel model)
		{
			// Add
			if (string.IsNullOrWhiteSpace(model.Uid) || !TbOrgUserExists(model.Uid))
			{
				model.Uid = EncodingHepler.NewID();
				var user = _mapper.Map<TbOrgUser>(model);
				await _context.TbOrgUsers.AddAsync(user);
			}
			else
			{
				var targetUser = await _context.TbOrgUsers.Include(x => x.TbOrgDeptUsers).FirstOrDefaultAsync(x => x.Uid == model.Uid);

				if (targetUser == null)
				{
					return NotFound();
				}

				var user = _mapper.Map<TbOrgUser>(model);
				_context.Entry(targetUser).CurrentValues.SetValues(user);
				targetUser.TbOrgDeptUsers = user.TbOrgDeptUsers;
			}

			try
			{
				await _context.SaveChangesWithLogAsync(_uid);
			}
			catch (DbUpdateException)
			{
				if (!TbOrgUserExists(model.Uid))
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

		private bool TbOrgUserExists(string id)
		{
			return (_context.TbOrgUsers?.Any(e => e.Uid == id)).GetValueOrDefault();
		}
	}
}
