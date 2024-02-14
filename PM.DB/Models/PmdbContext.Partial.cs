using Microsoft.EntityFrameworkCore;
using PMCore.Helpers;
using System.Text.Json;

namespace PMDB.Models
{
	public partial class PmdbContext : DbContext
	{
		public async Task<int> SaveChangesWithLogAsync(string uid)
		{
			var entries = ChangeTracker.Entries();
			var logList = new List<TbLog>();
			var UpdateTime = DateTime.Now;

			foreach (var entry in entries)
			{
				if (!entry.OriginalValues.Properties.Any(x => x.Name == "LogId"))
				{
					continue;
				}

				string logID = entry.OriginalValues.GetValue<string>("LogId");
				string? originalValues = null;
				string? currentValues = null;

				if (string.IsNullOrWhiteSpace(logID))
				{
					logID = EncodingHepler.NewID();
				}

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
						UpdateTime = UpdateTime,
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
