using AutoMapper;
using PMAPI.Models.Menu;
using PMAPI.Models.OrgDepts;
using PMAPI.Models.OrgUser;
using PMDB.Models;

namespace PMAPI.Models
{
	public class ModelProfile : Profile
	{
		public ModelProfile()
		{
			CreateMap<TbMenu, MenuViewModel>();
			CreateMap<VwOrgDept, OrgDeptsViewModel>();
			CreateMap<VwOrgCompany, CompanyViewModel>();
			CreateMap<CompanyModel, TbOrgDept>();
			CreateMap<TbOrgUser, OrgUserViewModel>();
		}
	}
}
