using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NetCoreConcepts.Dal;
using NetCoreConcepts.UtilidadesApi;
using NetCoreConcepts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static NetCoreConcepts.Models.LoginModels;

namespace NetCoreConcepts.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;
        
        UtilidadesApiss utils = new UtilidadesApiss();
        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        [Route("Account/Login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            UsuarioDal usuarioDal = new UsuarioDal(_config);
            var response = new Dictionary<string, string>();
            UsuarioModels usuario = new UsuarioModels();
            try
            {
                usuario = usuarioDal.ObtenerUsuario(request.Username);
            }
            catch (Exception)
            {
                response.Add("Error", "Hubo un problema al validar usuario.");
                return Ok(response);
            }
            if (!(request.Username == usuario.usuario && request.Password == utils.Decrypt(usuario.contrasena)))
            {
                response.Add("Error", "Invalid username or password");
                return Ok(response);
            }

            var roles = new string[] { "Role1", "Role2" };
            var token = GenerateJwtToken(request.Username, roles.ToList());
            return Ok(new LoginResponse()
            {
                Access_Token = token,
                UserName = request.Username
            });
        }

        private string GenerateJwtToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, username)
        };

            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_config["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _config["JwtIssuer"],
                _config["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpGet]
        [Route("Account/ObtenerUsuarios")]
        public async Task<string> ObtenerUsuarios()
        {
            UsuarioDal dal = new UsuarioDal(_config);
            List<UsuarioModels> usuarioList = new List<UsuarioModels>();
            try
            {

                usuarioList = await Task.Run(() => dal.ObtenerUsuarios());
                return JsonConvert.SerializeObject(usuarioList);

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject("99");
            }
        }
        [HttpPost]
        [Route("Account/IngresarUsuario")]
        public async Task<string> IngresarUsuario(UsuarioModels usuarioRequest)
        {
            UsuarioDal dal = new UsuarioDal(_config);
            UtilidadesApiss util = new UtilidadesApiss();
            usuarioRequest.contrasena = util.Encrypt(usuarioRequest.contrasena);
           /* List<UsuarioModels> usuarioList = new List<UsuarioModels>();*/
            UsuarioModels usuario = new UsuarioModels();
            
            try
            {
                usuario =dal.ObtenerUsuario(usuarioRequest.usuario);
                if(usuario.nombre_completo == null) {
                    await Task.Run(() => dal.CrearUsuario(usuarioRequest));
                    return JsonConvert.SerializeObject("ok");
                }
                else
                {
                    return JsonConvert.SerializeObject("usuario existe");
                }
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject("99");
            }
        }
    }
}
