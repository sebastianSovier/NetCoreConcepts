using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

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
        public async Task<string> TodosLosPaises()
        {
            PaisesDal dal = new PaisesDal(_config);
            List<PaisesModel> countriesList = new List<PaisesModel>();
            try
            {
                countriesList = await Task.Run(() => dal.ObtenerPaises());
                return JsonConvert.SerializeObject(countriesList);

            }catch (Exception ex)
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
                countriesList = await Task.Run(() => dal.ObtenerPaises());
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
                countriesList = await Task.Run(() => dal.ObtenerPaises());
                return JsonConvert.SerializeObject(countriesList);

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
                    countriesList = await Task.Run(() => dal.ObtenerPaises()); 
                    return JsonConvert.SerializeObject(countriesList);

                }
                catch (Exception ex)
                {
                    return JsonConvert.SerializeObject("99");
                }
            }

        }
}
