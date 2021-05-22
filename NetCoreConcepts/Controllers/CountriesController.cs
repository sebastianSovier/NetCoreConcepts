using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize()]
        [HttpPost]
        [Route("Countries/TodosLosPaises")]
        public async Task<string> TodosLosPaises()
        {
            List<CountriesModel> countriesList = new List<CountriesModel>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("https://restcountries.eu/rest/v2/all"))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        countriesList = JsonConvert.DeserializeObject<List<CountriesModel>>(apiResponse);
                    }
                }
                return JsonConvert.SerializeObject(countriesList);
            }catch (Exception)
            {
                return JsonConvert.SerializeObject("99");
            }
        }
        
    }
}
