using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Negocio;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Utilidades;

namespace NetCoreConcepts.Controllers
{
    [ApiController]
    public class CiudadesController : ControllerBase
    {
        private readonly IConfiguration _config;
        Dictionary<string, string> response = new Dictionary<string, string>();
        List<CiudadesModel> ciudadesList = new List<CiudadesModel>();
        private UtilidadesApiss utils = new UtilidadesApiss();



        public CiudadesController(IConfiguration config)
        {
            _config = config;
        }


        [Authorize()]
        [HttpPost]
        [Route("Ciudades/CiudadesPais")]
        [TypeFilter(typeof(FilterSessionAttribute))]
        public IActionResult ListarCiudades(UsuarioRequest request)
        {
            CiudadesBo bo = new CiudadesBo(_config);

            try
            {
                ciudadesList = bo.ObtenerCiudades(request);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
        [Authorize()]
        [HttpPost]
        [Route("Ciudades/IngresarCiudad")]
        [TypeFilter(typeof(FilterSessionAttribute))]
        public IActionResult IngresarCiudad(CiudadesModel ciudadRequest)
        {
            CiudadesBo bo = new CiudadesBo(_config);
            try
            {
                ciudadesList = bo.InsertarCiudad(ciudadRequest);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
        [Authorize()]
        [HttpPost]
        [Route("Ciudades/GetDataFromExcel")]
        [TypeFilter(typeof(FilterSessionAttribute))]
        public IActionResult GetDataFromExcel([FromBody] ExcelDataRequest request)
        {
            CiudadesBo bo = new CiudadesBo(_config);
            try
            {
                ciudadesList = bo.ImportarExcel(request);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
        [Authorize()]
        [HttpPost]
        [Route("Ciudades/ModificarCiudad")]
        [TypeFilter(typeof(FilterSessionAttribute))]
        public IActionResult ModificarCiudad(CiudadesModel ciudadRequest)
        {
            CiudadesBo bo = new CiudadesBo(_config);
            try
            {
                ciudadesList = bo.ModificarCiudad(ciudadRequest);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }

        [Authorize()]
        [HttpPost]
        [Route("Ciudades/EliminarCiudad")]
        [TypeFilter(typeof(FilterSessionAttribute))]
        public IActionResult EliminarCiudad(CiudadesModel ciudadRequest)
        {
            CiudadesBo bo = new CiudadesBo(_config);
            try
            {
                ciudadesList = bo.EliminarCiudad(ciudadRequest);
                return Ok(JsonConvert.SerializeObject(ciudadesList));

            }
            catch (Exception ex)
            {
                utils.createlogFile(ex.Message);
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
    }
}
