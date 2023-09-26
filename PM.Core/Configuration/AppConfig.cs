using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMCore.Configuration
{
	public class AppConfig
	{
		private IConfiguration _config;
		public AppConfig(IConfiguration config)
		{
			_config = config;
		}

		public string APIKey()
		{
			return _config["Key:PM.API"].ToString();
		}
	}
}
