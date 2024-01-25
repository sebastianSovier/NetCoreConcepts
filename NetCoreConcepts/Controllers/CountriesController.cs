using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Bo;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static NetCoreConcepts.Models.LoginModels;

namespace NetCoreConcepts.Controllers
{
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CountriesController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize()]
        [HttpPost]
        [Route("Countries/TodosLosPaises")]
        public IActionResult TodosLosPaises(UsuarioRequest request)
        {
            PaisesBo bo = new PaisesBo(_config);
            var response = new Dictionary<string, string>();
            List<PaisesModel> countriesList = new List<PaisesModel>();
            try
            {
                countriesList = bo.ObtenerPaises(request);
                return Ok(JsonConvert.SerializeObject(countriesList));

            }
            catch (Exception ex)
            {
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
        [Authorize()]
        [HttpPost]
        [Route("Countries/ObtenerPaisesPorFechas")]
        public IActionResult ObtenerPaisesPorFechas(UsuarioRequest request)
        {
            PaisesBo bo = new PaisesBo(_config);
            var response = new Dictionary<string, string>();
            List<PaisesModel> countriesList = new List<PaisesModel>();
            try
            {
                countriesList = bo.ObtenerPaisesPorFechas(request);
                return Ok(JsonConvert.SerializeObject(countriesList));

            }
            catch (Exception ex)
            {
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
        [Authorize()]
        [HttpPost]
        [Route("Countries/GetExcelPaises")]
        public IActionResult GetExcelPaises(UsuarioRequest request)
        {
            PaisesBo bo = new PaisesBo(_config);
            var response = new Dictionary<string, string>();
            List<PaisesModel> countriesList = new List<PaisesModel>();
            try
            {
                countriesList = bo.ObtenerPaises(request);
                return Ok(JsonConvert.SerializeObject(countriesList));

            }
            catch (Exception ex)
            {
                response.Add("Error", "Hubo un problema.");
                return StatusCode(500, response);
            }
        }
        [Authorize()]
        [HttpGet]
        [Route("Countries/CiudadesPais")]
        public async Task<string> ListarCiudades(string pais_id)
        {
            PaisesDal dal = new PaisesDal(_config);
            List<CiudadesModel> countriesList = new List<CiudadesModel>();
            try
            {
                countriesList = await Task.Run(() => dal.ObtenerCiudades(pais_id));
                return JsonConvert.SerializeObject(countriesList);

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject("99");
            }
        }
        [Authorize()]
        [HttpPost]
        [Route("Countries/IngresarCiudad")]
        public async Task<string> IngresarCiudad(CiudadesModel ciudadRequest)
        {
            PaisesDal dal = new PaisesDal(_config);
            List<CiudadesModel> ciudadesList = new List<CiudadesModel>();
            try
            {
                dal.InsertarCiudad(ciudadRequest);
                ciudadesList = await Task.Run(() => dal.ObtenerCiudades(ciudadRequest.pais_id.ToString()));
                return JsonConvert.SerializeObject(ciudadesList);

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject("99");
            }
        }

        [Authorize()]
        [HttpPost]
        [Route("Countries/IngresarPais")]
        public async Task<string> IngresarPais(PaisesModel paisRequest)
        {
            PaisesDal dal = new PaisesDal(_config);
            List<PaisesModel> countriesList = new List<PaisesModel>();
            try
            {
                dal.InsertarPaises(paisRequest);
                countriesList = await Task.Run(() => dal.ObtenerPaises(paisRequest.pais_id));
                return JsonConvert.SerializeObject(countriesList);

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject("99");
            }
        }
        [Authorize()]
        [HttpPut]
        [Route("Countries/ModificarPais")]
        public async Task<string> ModificarPais(PaisesModel paisRequest)
        {
            PaisesDal dal = new PaisesDal(_config);
            List<PaisesModel> countriesList = new List<PaisesModel>();
            try
            {
                dal.ModificarPais(paisRequest);
                countriesList = await Task.Run(() => dal.ObtenerPaises(paisRequest.pais_id));
                return JsonConvert.SerializeObject(countriesList);

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject("99");
            }
        }
        [Authorize()]
        [HttpPut]
        [Route("Countries/ModificarCiudad")]
        public async Task<string> ModificarCiudad(CiudadesModel ciudadRequest)
        {
            PaisesDal dal = new PaisesDal(_config);
            List<CiudadesModel> ciudadList = new List<CiudadesModel>();
            try
            {
                dal.ModificarCiudad(ciudadRequest);
                ciudadList = await Task.Run(() => dal.ObtenerCiudades(ciudadRequest.pais_id.ToString()));
                return JsonConvert.SerializeObject(ciudadList);

            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject("99");
            }
        }

            [Authorize()]
            [HttpDelete]
            [Route("Countries/EliminarPais")]
            public async Task<string> EliminarPais(string pais_id)
            {
                PaisesDal dal = new PaisesDal(_config);
                List<PaisesModel> countriesList = new List<PaisesModel>();
            try
                {
                    dal.EliminarPais(pais_id);
                    countriesList = await Task.Run(() => dal.ObtenerPaises(1)); 
                    return JsonConvert.SerializeObject(countriesList);

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject("99");
                }
            }

        
    [Authorize()]
    [HttpDelete]
    [Route("Countries/EliminarCiudad")]
    public async Task<string> EliminarCiudad(string ciudad_id,string pais_id)
    {
        PaisesDal dal = new PaisesDal(_config);
        List<CiudadesModel> ciudadList = new List<CiudadesModel>();
        try
        {
            dal.EliminarCiudad(ciudad_id);
                ciudadList = await Task.Run(() => dal.ObtenerCiudades(pais_id));
            return JsonConvert.SerializeObject(ciudadList);

        }
        catch (Exception ex)
        {
            return JsonConvert.SerializeObject("99");
        }
    }

}
}
