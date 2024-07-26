﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Negocio;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;
using System;
using System.Collections.Generic;
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
        public IActionResult Login(LoginRequest request)
        {
            LoginBo Login = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            UsuarioModels usuario = new UsuarioModels();
            try
            {

                usuario = Login.ObtenerUsuario(request.Username);

                if (!(request.Username == usuario.usuario) || !(utils.ComparePassword(request.Password, usuario.contrasena)))
                {
                    response.Add("Error", "Invalid username or password");
                    return StatusCode(403, response);
                }
                SessionModels req = new SessionModels();
                req.usuario = usuario.usuario;
                req.usuario_id = usuario.usuario_id;
                SessionModels session = Login.ObtenerSessionUsuario(req);
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
                        Login.UpdateSessionUser(req);
                    }
                }
                else
                {
                    Login.CrearSession(req);
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
        [HttpPost]
        [Route("Account/IngresarUsuario")]
        public IActionResult IngresarUsuario(UsuarioModels usuarioRequest)
        {
            LoginBo Login = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            UsuarioModels usuario = new UsuarioModels();

            try
            {
                usuario = Login.ObtenerUsuario(usuarioRequest.usuario);
                if (usuario.nombre_completo == null)
                {
                    Login.CrearUsuario(usuarioRequest);
                    return Ok(new LoginResponse
                    {
                        auth = true

                    });
                }
                else
                {
                    return Ok();
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
        public IActionResult CrearSession(SessionModels usuarioRequest)
        {
            LoginBo Login = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            UsuarioModels usuario = new UsuarioModels();

            try
            {
                Login.CrearSession(usuarioRequest);
                return Ok();


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al crear usuario.");
                return StatusCode(500, response);
            }
        }

        [HttpPost]
        [Route("Session/ActualizarSession")]
        public IActionResult ActualizarSession(SessionModels usuarioRequest)
        {
            LoginBo Login = new LoginBo(_config);
            var response = new Dictionary<string, string>();

            try
            {
                Login.UpdateSessionLogoutUser(usuarioRequest);
                return Ok();


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al crear usuario.");
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        [Route("Session/ObtenerSession")]
        public IActionResult ObtenerSession(SessionModels usuarioRequest)
        {
            LoginBo Login = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            try
            {
                SessionModels resp = Login.ObtenerSessionUsuario(usuarioRequest);
                return Ok(resp);


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al crear usuario.");
                return StatusCode(500, response);
            }
        }
        [HttpPost]
        [Route("Session/CierreSessionesInactivas")]
        public IActionResult CierreSessionesInactivas()
        {
            LoginBo Login = new LoginBo(_config);
            var response = new Dictionary<string, string>();
            try
            {
                Login.ObtenerSessionesUsuariosInactivos();
                return Ok();


            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema al crear usuario.");
                return StatusCode(500, response);
            }
        }
    }
}
