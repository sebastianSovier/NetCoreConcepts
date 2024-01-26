using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Bo;
using Org.BouncyCastle.Asn1.Ocsp;
using Negocio;

namespace NetCoreConcepts.Controllers
{
    [ApiController]
    public class CiudadesController : ControllerBase
    {
        private readonly IConfiguration _config;
        Dictionary<string, string> response = new Dictionary<string, string>();

        public CiudadesController(IConfiguration config)
        {
            _config = config;
        }


        [Authorize()]
        [HttpPost]
        [Route("Ciudades/CiudadesPais")]
        public IActionResult ListarCiudades(UsuarioRequest request)
        {
            CiudadesBo bo = new CiudadesBo(_config);
            List<CiudadesModel> ciudadesList = new List<CiudadesModel>();
            try
            {
                ciudadesList = bo.ObtenerCiudades(request);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
        [Authorize()]
        [HttpPost]
        [Route("Ciudades/IngresarCiudad")]
        public IActionResult IngresarCiudad(CiudadesModel ciudadRequest)
        {
            CiudadesBo bo = new CiudadesBo(_config);
            List<CiudadesModel> ciudadesList = new List<CiudadesModel>();
            try
            {
                ciudadesList = bo.InsertarCiudad(ciudadRequest);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }

        [Authorize()]
        [HttpPost]
        [Route("Ciudades/ModificarCiudad")]
        public IActionResult ModificarCiudad(CiudadesModel ciudadRequest)
        {
            CiudadesBo bo = new CiudadesBo(_config);
            List<CiudadesModel> ciudadesList = new List<CiudadesModel>();
            try
            {
                ciudadesList = bo.ModificarCiudad(ciudadRequest);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }

        [Authorize()]
        [HttpPost]
        [Route("Ciudades/EliminarCiudad")]
        public IActionResult EliminarCiudad(CiudadesModel ciudadRequest)
        {
            CiudadesBo bo = new CiudadesBo(_config);
            List<CiudadesModel> ciudadesList = new List<CiudadesModel>();
            try
            {
                ciudadesList = bo.EliminarCiudad(ciudadRequest);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
    }
}
