using System.Security.Cryptography;
using System.Text;

namespace PMCore.Helpers
{
	public class EncodingHepler
	{
		public static string ComputeHMACSHA256(string data, string key)
		{
			var keyBytes = Encoding.UTF8.GetBytes(key);
			using (var hmacSHA = new HMACSHA256(keyBytes))
			{
				var dataBytes = Encoding.UTF8.GetBytes(data);
				var hash = hmacSHA.ComputeHash(dataBytes, 0, dataBytes.Length);
				return BitConverter.ToString(hash).Replace("-", "").ToUpper();
			}
		}
	}
}
