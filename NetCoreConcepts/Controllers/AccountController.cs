using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreConcepts.UtilidadesApi;
using NetCoreConcepts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NetCoreConcepts.Models.LoginModels;
using NetCoreConcepts.Dal;
using Negocio;

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
        public IActionResult Login(LoginRequest request)
        {
            LoginBo Login = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            UsuarioModels usuario = new UsuarioModels();
            try
            {

                usuario = Login.ObtenerUsuario(request.Username);

                if (!(request.Username == usuario.usuario && utils.ComparePassword(request.Password,usuario.contrasena)))
            {
                response.Add("Error", "Invalid username or password");
                return StatusCode(403,response);
            }

            var token = utils.GenerateJwtToken(request.Username, _config);
            return Ok(new LoginResponse()
            {
                access_Token = token,
                auth = true,
                id = usuario.usuario_id

            });
            }
            catch (Exception ex)
            {
                response.Add("Error", "Hubo un problema al validar usuario.");
                return StatusCode(500,response);
            }
        }
        [HttpPost]
        [Route("Account/IngresarUsuario")]
        public async Task<string> IngresarUsuario(UsuarioModels usuarioRequest)
        {
            UsuarioDal dal = new UsuarioDal(_config);
            UtilidadesApiss util = new UtilidadesApiss();
            usuarioRequest.contrasena = usuarioRequest.contrasena;
           /* List<UsuarioModels> usuarioList = new List<UsuarioModels>();*/
            UsuarioModels usuario = new();
            
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
