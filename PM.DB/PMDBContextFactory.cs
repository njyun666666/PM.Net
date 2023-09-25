using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PMCore.Configuration;
using PMDB.Models;

namespace PMDB
{
	public class PMDBContextFactory : IDesignTimeDbContextFactory<PmdbContext>
	{
		public PmdbContext CreateDbContext(string[] args)
		{
			var builder = new DbContextOptionsBuilder<PmdbContext>();
			var configuration = AppConfigurations.BuildConfiguration();
			builder.UseMySql(configuration.GetConnectionString("PMDB"), Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

			return new PmdbContext(builder.Options);
		}
	}
}
// Scaffold-DbContext Name=ConnectionStrings:PMDB Pomelo.EntityFrameworkCore.MySql -OutputDir Models -force -NoOnConfiguring
