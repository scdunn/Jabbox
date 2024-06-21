using Jabbox.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jabbox.API
{
    /// <summary>
    /// Generates and handles Java Web Token Security
    /// </summary>
    public class TokenHandler
    {
        private readonly IConfigurationSection _tokenSettings;

        public TokenHandler(IConfiguration configuration)
        {
            _tokenSettings = configuration.GetSection("TokenSettings");
        }

        /// <summary>
        /// Returns signing credentials using security key
        /// </summary>
        /// <returns></returns>
        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_tokenSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Returns claims for an individual account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public List<Claim> GetClaims(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, account.Username)
            };

            return claims;
        }

        /// <summary>
        /// Returns Token options (issuer, audience and expiration to apply to token
        /// </summary>
        /// <param name="signingCredentials"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _tokenSettings["validIssuer"],
                audience: _tokenSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_tokenSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }

        /// <summary>
        /// Returns token for an individual account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public string GetToken(Account account)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(account);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }
    }
}
