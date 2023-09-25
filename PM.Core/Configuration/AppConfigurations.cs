using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMCore.Configuration
{
	public static class AppConfigurations
	{
		public static IConfigurationRoot BuildConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(GetProjectFolder("PM.API"))
				.AddJsonFile("appsettings.json");

			return builder.Build();
		}

		public static string GetProjectFolder(string projectName)
		{
			var currentDirectoryPath = Directory.GetCurrentDirectory();
			var directoryInfo = new DirectoryInfo(currentDirectoryPath);
			while (!DirectoryContains(directoryInfo.FullName, "PM.Net.sln"))
			{
				if (directoryInfo.Parent == null)
				{
					throw new Exception("Could not find content root folder!");
				}

				directoryInfo = directoryInfo.Parent;
			}

			var webMvcFolder = Path.Combine(directoryInfo.FullName, projectName);
			if (Directory.Exists(webMvcFolder))
			{
				return webMvcFolder;
			}

			throw new Exception("Could not find root folder of the project!");
		}

		private static bool DirectoryContains(string directory, string fileName)
		{
			return Directory.GetFiles(directory).Any(filePath => string.Equals(Path.GetFileName(filePath), fileName));
		}

	}

}
