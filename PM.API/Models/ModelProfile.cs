using AutoMapper;
using PMAPI.Models.Menu;
using PMDB.Models;

namespace PMAPI.Models
{
	public class ModelProfile : Profile
	{
		public ModelProfile()
		{
			CreateMap<TbMenu, MenuViewModel>();
		}
	}
}
