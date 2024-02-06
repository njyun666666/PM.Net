using Microsoft.Extensions.Configuration;

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
