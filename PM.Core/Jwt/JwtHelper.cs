using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PMCore.Jwt
{
	public class JwtHelper
	{
		private readonly IConfiguration _config;

		public JwtHelper(IConfiguration config)
		{
			_config = config;
		}

		public string GenerateToken(string userName, List<Claim> claims, int expireMinutes = 30)
		{
#if DEBUG
			expireMinutes = 365 * 24 * 60;
#endif

			var issuer = _config.GetValue<string>("JwtSettings:Issuer");
			var signKey = _config.GetValue<string>("JwtSettings:SignKey");
			claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userName)); // User.Identity.Name
			claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())); // JWT ID

			var userClaimsIdentity = new ClaimsIdentity(claims);

			// Create a SymmetricSecurityKey for JWT Token signatures
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signKey));

			// HmacSha256 MUST be larger than 128 bits, so the key can't be too short. At least 16 and more characters.
			// https://stackoverflow.com/questions/47279947/idx10603-the-algorithm-hs256-requires-the-securitykey-keysize-to-be-greater
			var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			// Create SecurityTokenDescriptor
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Issuer = issuer,
				Subject = userClaimsIdentity,
				Expires = DateTime.Now.AddMinutes(expireMinutes),
				SigningCredentials = signingCredentials
			};

			// Generate a JWT securityToken, than get the serialized Token result (string)
			var tokenHandler = new JwtSecurityTokenHandler();
			var securityToken = tokenHandler.CreateToken(tokenDescriptor);
			var serializeToken = tokenHandler.WriteToken(securityToken);

			return serializeToken;
		}
	}

}
