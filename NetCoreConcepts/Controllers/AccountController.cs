using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Negocio;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;
using System;
using System.Collections.Generic;
using static NetCoreConcepts.Models.LoginModels;

namespace NetCoreConcepts.Controllers
{
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;

        private UtilidadesApiss utils = new UtilidadesApiss();
        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Account/Login")]
        public IActionResult Login(LoginRequest request)
        {
            LoginBo Login = new LoginBo(_config);
            SessionBo Sesion = new SessionBo(_config);
            PasswordBo Password = new PasswordBo(_config);
            var response = new Dictionary<string, string>();
            UsuarioModels usuario = new UsuarioModels();
            PasswordModels password = new PasswordModels();
            try
            {

                usuario = Login.ObtenerUsuario(request.Username);
                password = Password.ObtenerPassword(usuario.usuario_id.ToString());
                if (!(request.Username == usuario.usuario) || !(utils.ComparePassword(request.Password, password.password)))
                {
                    response.Add("Error", "Invalid username or password");
                    return StatusCode(403, response);
                }
                SessionModels req = new SessionModels();
                req.usuario = usuario.usuario;
                req.usuario_id = usuario.usuario_id;
                SessionModels session = Sesion.ObtenerSessionUsuario(req);
                if (session.usuario_id > 0)
                {
                    if (session.user_activo.Equals("ACTIVO"))
                    {
                        response.Add("Error", "usuario online");
                        return StatusCode(200, response);
                    }
                    else
                    {
                        req.user_activo = "ACTIVO";
                        Sesion.UpdateSessionUser(req);
                    }
                }
                else
                {
                    Sesion.CrearSession(req);
                }
                string token = utils.GenerateJwtToken(request.Username, _config);
                return Ok(new LoginResponse()
                {
                    access_Token = token,
                    auth = true,
                    id = usuario.usuario_id,
                    correo = usuario.correo

                });
            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al validar usuario.");
                return StatusCode(500, response);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("Account/IngresarUsuario")]
        public IActionResult IngresarUsuario(UsuarioModels request)
        {
            LoginBo Login = new LoginBo(_config);
            PasswordBo Password = new PasswordBo(_config);
            var response = new Dictionary<string, string>();
            UsuarioModels usuario = new UsuarioModels();

            try
            {

                usuario = Login.ObtenerUsuario(request.usuario);
                if (usuario.nombre_completo == null)
                {
                    Login.CrearUsuario(request);
                    UsuarioModels usuarioResp = Login.ObtenerUsuario(request.usuario);
                    PasswordModels passwordRequest = new PasswordModels();
                    passwordRequest.usuario_id = usuarioResp.usuario_id;
                    passwordRequest.password = request.contrasena;
                    Password.CrearPassword(passwordRequest);
                    return Ok(new LoginResponse
                    {
                        auth = true

                    });
                }
                else
                {
                    return Ok(new LoginResponse
                    {
                        auth = false

                    });
                }

            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al crear usuario.");
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        [Route("Session/CrearSession")]
        public IActionResult CrearSession(SessionModels request)
        {
            SessionBo Sesion = new SessionBo(_config);
            var response = new Dictionary<string, string>();
            UsuarioModels usuario = new UsuarioModels();

            try
            {
                Sesion.CrearSession(request);
                return Ok();


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al crear sesion.");
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Session/ActualizarSession")]
        public IActionResult ActualizarSession(SessionModels request)
        {
            SessionBo Sesion = new SessionBo(_config);
            var response = new Dictionary<string, string>();

            try
            {
                Sesion.UpdateSessionLogoutUser(request);
                return Ok();


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al actualizar sesion.");
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        [Route("Session/ObtenerSession")]
        public IActionResult ObtenerSession(SessionModels usuarioRequest)
        {
            SessionBo Sesion = new SessionBo(_config);
            var response = new Dictionary<string, string>();
            try
            {
                SessionModels resp = Sesion.ObtenerSessionUsuario(usuarioRequest);
                return Ok(resp);


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al obtener sesion.");
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Session/CierreSessionesInactivas")]
        public IActionResult CierreSessionesInactivas()
        {
            SessionBo Sesion = new SessionBo(_config);
            var response = new Dictionary<string, string>();
            try
            {
                Sesion.ObtenerSessionesUsuariosInactivos();
                return Ok();


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al cerrar sesiones activas.");
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Account/CodigoRecuperacion")]
        public IActionResult CodigoRecuperacionPassword(PasswordModels passwordRequest)
        {
            PasswordBo Password = new PasswordBo(_config);
            LoginBo Usuario = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            try
            {
                UsuarioModels usuario = Usuario.ObtenerUsuario(passwordRequest.usuario);

                if (usuario != null)
                {
                    passwordRequest.usuario_id = usuario.usuario_id;
                    PasswordModels passwordResponse = Password.GenerarCodigoRecuperacion(passwordRequest);
                    passwordResponse.correo = usuario.correo;
                    return Ok(passwordResponse);
                }
                response.Add("Error", "Usuario no existe");
                return StatusCode(403, response);


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al cerrar sesiones activas.");
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Account/ValidaCodigo")]
        public IActionResult ValidaCodigoRecuperacion(PasswordModels passwordRequest)
        {
            PasswordBo Password = new PasswordBo(_config);
            LoginBo Usuario = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            try
            {
                UsuarioModels usuario = Usuario.ObtenerUsuario(passwordRequest.usuario);
                PasswordModels passwordResponse = Password.ObtenerPassword(usuario.usuario_id.ToString());
                if (passwordResponse.cod_recover_password.Equals(passwordRequest.cod_recover_password))
                {
                    return Ok(new LoginResponse
                    {
                        auth = true

                    });
                }
                else
                {
                    response.Add("Error", "Codigo invalido");
                    return StatusCode(403, response);
                }



            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al cerrar sesiones activas.");
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Account/CambioPassword")]
        public IActionResult CambioPassword(PasswordModels passwordRequest)
        {
            PasswordBo Password = new PasswordBo(_config);
            LoginBo Usuario = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            try
            {
                UsuarioModels usuario = Usuario.ObtenerUsuario(passwordRequest.usuario);
                PasswordModels passwordResp = Password.ObtenerPassword(usuario.usuario_id.ToString());
                passwordRequest.usuario_id = usuario.usuario_id;
                Password.CambioPassword(passwordRequest);
                return Ok(new LoginResponse
                {
                    auth = true

                });


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al cerrar sesiones activas.");
                return StatusCode(500, response);
            }
        }
    }
}
