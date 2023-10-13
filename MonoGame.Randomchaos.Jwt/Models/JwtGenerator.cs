
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace MonoGame.Randomchaos.Jwt.Models
{
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A Java Web Token generator. </summary>
    ///
    /// <remarks>   Charles Humphrey, 14/10/2023. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class JwtGenerator
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Generates a token. </summary>
        ///
        /// <remarks>   Charles Humphrey, 10/10/2023. </remarks>
        ///
        /// <param name="configuration">    The configuration. </param>
        /// <param name="subject">          The subject. </param>
        /// <param name="name">             The name. </param>
        /// <param name="roles">            The roles. </param>
        /// <param name="nbf">              (Optional) The nbf. </param>
        /// <param name="exp">              (Optional) The exponent. </param>
        /// <param name="duration">         (Optional) The duration. </param>
        ///
        /// <returns>   The token. </returns>
        ///-------------------------------------------------------------------------------------------------

        public virtual string GenerateToken(JwtConfiguration configuration, string subject, string name, List<string> roles, DateTime? nbf = null, DateTime? exp = null, double duration = 30)
        {
            Dictionary<string, string> claims = new Dictionary<string, string>()
            {
                { JwtRegisteredClaimNames.Sub, subject},
                { JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()},
                { JwtRegisteredClaimNames.UniqueName,name},
                { JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString()},
                { JwtRegisteredClaimNames.Exp, exp == null ? DateTimeOffset.Now.AddMinutes(duration).ToUnixTimeSeconds().ToString() : new DateTimeOffset(exp.Value).ToUnixTimeSeconds().ToString() },
                { JwtRegisteredClaimNames.Nbf, nbf == null ? DateTimeOffset.Now.ToUnixTimeSeconds().ToString() : new DateTimeOffset(nbf.Value).ToUnixTimeSeconds().ToString() },
            };


            return GenerateJWTBearerString(claims, roles, configuration.Key, configuration.Issuer, configuration.Audience);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Generates a jwt bearer string. </summary>
        ///
        /// <remarks>   Charles Humphrey, 10/10/2023. </remarks>
        ///
        /// <param name="additonalClaims">  The additonal claims. </param>
        /// <param name="roles">            The roles. </param>
        /// <param name="secret">           The secret. </param>
        /// <param name="iss">              The iss. </param>
        /// <param name="aud">              The aud. </param>
        ///
        /// <returns>   The jwt bearer string. </returns>
        ///-------------------------------------------------------------------------------------------------

        protected string GenerateJWTBearerString(Dictionary<string, string> additonalClaims, List<string> roles, string secret, string iss, string aud)
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();

            foreach (string claimName in additonalClaims.Keys)
                claims.Add(new Claim(claimName, additonalClaims[claimName]));

            if (roles != null)
            {
                foreach (string role in roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));
            }

            System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwtToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                        iss,
                        aud,
                        claims.ToArray(),
                        notBefore: DateTime.Now,
                        signingCredentials: creds);

            return new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Validates the token. </summary>
        ///
        /// <remarks>   Charles Humphrey, 10/10/2023. </remarks>
        ///
        /// <param name="configuration">    The configuration. </param>
        /// <param name="token">            The token. </param>
        ///
        /// <returns>   True if it succeeds, false if it fails. </returns>
        ///-------------------------------------------------------------------------------------------------

        public bool ValidateToken(JwtConfiguration configuration, string token)
        {
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration.Issuer,
                ValidateAudience = true,
                ValidAudience = configuration.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = new[] { new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key)) },
                ValidateLifetime = true
            };

            try
            {
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                return true;
            }
            catch (SecurityTokenValidationException ex)
            {
                // Log the reason why the token is not valid
                return false;
            }
        }
    }
}
