using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace PMDB.Models
{
	public partial class PmdbContext : DbContext
	{
		public async Task<int> SaveChangesWithLogAsync(string uid)
		{
			var entries = ChangeTracker.Entries();
			string logID = Guid.NewGuid().ToString().Replace("-", "");
			var logList = new List<TbLog>();

			foreach (var entry in entries)
			{
				string? originalValues = null;
				string? currentValues = null;

				switch (entry.State)
				{
					case EntityState.Added:
						currentValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
						break;

					case EntityState.Deleted:
						originalValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
						break;

					case EntityState.Modified:
						Dictionary<string, object?> oValue = new Dictionary<string, object?>();
						Dictionary<string, object?> cValue = new Dictionary<string, object?>();

						foreach (var prop in entry.Properties)
						{
							if (prop.IsModified)
							{
								var name = prop.Metadata.Name;
								oValue.Add(name, prop.OriginalValue);
								cValue.Add(name, prop.CurrentValue);
							}
						}

						originalValues = JsonSerializer.Serialize(oValue);
						currentValues = JsonSerializer.Serialize(cValue);

						break;
					default:
						break;
				}

				if (entry.State != EntityState.Unchanged)
				{
					entry.CurrentValues.SetValues(new { LogId = logID });

					logList.Add(new TbLog
					{
						LogId = logID,
						Uid = uid,
						UpdateTime = DateTime.Now,
						UpdateTable = entry.Metadata.GetTableName(),
						LogState = entry.State.ToString(),
						OriginalValues = originalValues,
						CurrentValues = currentValues
					});
				}
			}

			if (logList.Any())
			{
				this.TbLogs.AddRange(logList);
			}

			return await this.SaveChangesAsync();
		}
	}
}
