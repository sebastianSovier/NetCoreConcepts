using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NetCoreConcepts.UtilidadesApi
{
    public class UtilidadesApiss
    {
        public UtilidadesApiss()
        {

        }
        public bool ComparePassword(string paswordReq, string passwordhash)
        {
            bool result = BCrypt.Net.BCrypt.Verify(paswordReq, passwordhash);

            return result;
        }

        public string GenerateJwtToken(string username, IConfiguration _config)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, username)
        };
            var roles = new string[] { "Admin", "User" };

            roles.ToList().ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_config["JwtExpireDays"]!));

            var token = new JwtSecurityToken(
                _config["JwtIssuer"],
                _config["JwtIssuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtDurationInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public void createlogFile(string logMessage)
        {
            /* File.Delete("Log//Log-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + ".txt");
             using (StreamWriter w = File.AppendText("Log//Log-"+DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) +".txt"))
             {
                 WriteLog(logMessage, w);
             }

             using (StreamReader r = File.OpenText("Log//Log-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + ".txt"))
             {*/
            //ImprmLog(r);
            Console.WriteLine(logMessage);
            //}
        }
        public static void WriteLog(string logMessage, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
            w.WriteLine("  :");
            w.WriteLine($"  :{logMessage}");
            w.WriteLine("-------------------------------");
        }

        public static void ImprmLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()!) != null)
            {
                Console.WriteLine(line);
            }
        }


    }
}
