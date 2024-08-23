using Datos;
using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;
using System.Data;

namespace Negocio
{
    public class CiudadesBo
    {

        private UtilidadesApiss utils = new UtilidadesApiss();

        private readonly IConfiguration _config;

        public CiudadesBo(IConfiguration config)
        {
            _config = config;

        }


        public CiudadesBo() { }

        public List<CiudadesModel>? ObtenerCiudades(UsuarioRequest request)
        {
            CiudadesDal ciudadesDal = new CiudadesDal(_config);
            LoginBo Login = new LoginBo(_config);
            SessionBo Session = new SessionBo(_config);
            SessionModels sessionReq = new SessionModels();
            ;
            sessionReq.usuario_id = Login.ObtenerUsuario(request.usuario!).usuario_id;
            SessionModels session = Session.ObtenerSessionUsuario(sessionReq);
            if (session.user_activo!.Equals("ACTIVO") && !utils.validTimeSession(session.fecha_actividad!))
            {
                Session.UpdateSessionUser(session);
                List<CiudadesModel> ciudades = ciudadesDal.ObtenerCiudades(request.pais_id);
                return ciudades;

            }
            else
            {
                return null;
            }


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
        public List<CiudadesModel>? ImportarExcel(ExcelDataRequest request)
        {
            CiudadesDal ciudadesDal = new CiudadesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(request.usuario!);

            if (usuario != null)
            {
                LecturaExcelCiudad(usuario, request.base64string!, request.pais_id);
                List<CiudadesModel> ciudades = ciudadesDal.ObtenerCiudades(request.pais_id);

                return ciudades;
            }
            else
            {
                return null;
            }

        }
        private void LecturaExcelCiudad(UsuarioModels usuario, string file, long pais_id)
        {
            List<CiudadesModel> ciudades = new();
            CiudadesDal ciudadesDal = new CiudadesDal(_config);
            try
            {
                DataTable dataExcel = utils.ConvertExcel(file);
                for (int i = 1; i <= dataExcel.Rows.Count - 1; i++)
                {
                    CiudadesModel ciudad = new();
                    ciudad.pais_id = pais_id;
                    ciudad.nombre_ciudad = dataExcel.Rows[i][0].ToString();
                    ciudad.poblacion = dataExcel.Rows[i][1].ToString();
                    ciudad.region = dataExcel.Rows[i][2].ToString();
                    ciudad.latitud = dataExcel.Rows[i][3].ToString();
                    ciudad.longitud = dataExcel.Rows[i][4].ToString();

                    ciudades.Add(ciudad);
                }
                foreach (CiudadesModel ciudad in ciudades)
                {
                    ciudadesDal.InsertarCiudad(ciudad);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
