namespace PMAPI.Services.IServices
{
	public interface IAuthService
	{
		Task<bool> IsAdmin(string uid);
		Task<bool> IsOrgAdmin(string rootDid, string uid);
	}
}
