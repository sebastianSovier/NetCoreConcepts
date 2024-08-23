﻿using ExcelDataReader;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        public int? ValidateToken(string token, IConfiguration _config)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtKey"]!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }
        public void createlogFile(string logMessage)
        {
            using (StreamWriter w = File.AppendText("Log//Log-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + ".txt"))
            {
                WriteLog(logMessage, w);
            }

            using (StreamReader r = File.OpenText("Log//Log-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) + ".txt"))
            {
                ImprmLog(r);
                Console.WriteLine(logMessage);
            }
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

        public DataTable ConvertExcel(string file)
        {
            byte[] byteArray = Convert.FromBase64String(file);
            using MemoryStream memoryStream = new MemoryStream(byteArray);
            using BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);
            byte[] data = binaryReader.ReadBytes(byteArray.Length);
            Console.WriteLine(BitConverter.ToString(data));
            Stream stream = new MemoryStream(data);
            IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream);

            var result = reader.AsDataSet();
            DataTable dataExcel = result.Tables[0];
            return dataExcel;
        }
        public bool validTimeSession(string fecha_actividad)
        {

            DateTime utcTime = DateTime.UtcNow;
            TimeZoneInfo chileTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific SA Standard Time");
            DateTime chileTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, chileTimeZone);
            TimeSpan timeDifference = chileTime - DateTime.Parse(fecha_actividad!);
            double minutesPassed = timeDifference.TotalMinutes;
            double secondsPassed = timeDifference.TotalSeconds;
            if (minutesPassed > 5 || (minutesPassed == 5 && secondsPassed > 0))
            {
                return true;
            }



            else
            {
                return false;
            }
        }
        public string generateRandomNumber()
        {

            Random rnd = new Random();
            Int64 number = rnd.NextInt64(1000000000, 9999999999);
            return number.ToString();
        }
    }
}
