using Datos;
using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CiudadesBo
    {
        private readonly IConfiguration _config;

        public CiudadesBo(IConfiguration config)
        {
            _config = config;

        }


        public CiudadesBo() { }

        public List<CiudadesModel>? ObtenerCiudades(UsuarioRequest request)
        {
            CiudadesDal ciudadesDal = new CiudadesDal(_config);
            List<CiudadesModel> ciudades = ciudadesDal.ObtenerCiudades(request.pais_id);
            return ciudades;

        }
        public List<CiudadesModel>? InsertarCiudad(CiudadesModel request)
        {
            CiudadesDal ciudadesDal = new CiudadesDal(_config);
            ciudadesDal.InsertarCiudad(request);
            List<CiudadesModel> ciudades = ciudadesDal.ObtenerCiudades(request.pais_id);
            return ciudades;


        }
        public List<CiudadesModel>? ModificarCiudad(CiudadesModel request)
        {
            CiudadesDal ciudadesDal = new CiudadesDal(_config);

            ciudadesDal.ModificarCiudad(request);
            List<CiudadesModel> ciudades = ciudadesDal.ObtenerCiudades(request.pais_id);

            return ciudades;

        }
        public List<CiudadesModel>? EliminarCiudad(CiudadesModel request)
        {
            CiudadesDal ciudadesDal = new CiudadesDal(_config);
            ciudadesDal.EliminarCiudad(request.ciudad_id);
            List<CiudadesModel> ciudades = ciudadesDal.ObtenerCiudades(request.pais_id);

            return ciudades;

        }
    }
}
