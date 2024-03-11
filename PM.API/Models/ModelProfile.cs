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
			CreateMap<MenuViewModel, AuthMenuViewModel>();
			CreateMap<VwOrgDept, OrgDeptsViewModel>();
			CreateMap<VwOrgCompany, CompanyViewModel>();
			CreateMap<CompanyModel, TbOrgDept>();
			CreateMap<TbOrgUser, OrgUserViewModel>();
			CreateMap<VwOrgCompanyUser, OrgUserDeptModel>();
			CreateMap<OrgUserModel, TbOrgUser>().ForMember(dest => dest.TbOrgDeptUsers, opt => opt.MapFrom(src => src.Depts));
			CreateMap<OrgUserDeptModel, TbOrgDeptUser>();
		}
	}
}
