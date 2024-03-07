using Microsoft.EntityFrameworkCore;
using PMAPI.Services.IServices;
using PMCore.Configuration;
using PMDB.Models;

namespace PMAPI.Services
{
	public class AuthService : IAuthService
	{
		private readonly PmdbContext _context;

		public AuthService(PmdbContext context)
		{
			_context = context;
		}
		public async Task<bool> IsAdmin(string uid)
		{
			return await _context.TbOrgRoleUsers.AnyAsync(x => x.Uid == uid && x.Rid == AppConst.Role.Administrator);
		}

		public async Task<bool> IsOrgAdmin(string rootDid, string uid)
		{
			if (await IsAdmin(uid))
			{
				return true;
			}

			return await _context.TbOrgRoleUsers.AnyAsync(x => x.Uid == uid && x.Rid == AppConst.Role.Organization && x.RootDid == rootDid);
		}


	}
}
