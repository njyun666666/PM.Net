namespace PMAPI.Services.IServices
{
	public interface IAuthService
	{
		Task<bool> CheckOrgAdmin(string rootDid, string uid);
	}
}
