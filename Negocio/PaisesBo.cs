using Datos;
using Microsoft.Extensions.Configuration;
using Negocio;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;
using Newtonsoft.Json;
using System.Data;

namespace NetCoreConcepts.Bo
{
    public class PaisesBo
    {
        UtilidadesApiss utils = new UtilidadesApiss();
        private readonly IConfiguration _config;
        public PaisesBo(IConfiguration config)
        {
            _config = config;

        }
        public List<PaisesModel>? ObtenerPaises(UsuarioRequest request)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(request.usuario!);

            if (usuario != null)
            {
                if (usuario.usuario!.Equals("admin"))
                {
                    List<PaisesModel> paises = paisesDal.ObtenerPaises(0);

                    return paises;
                }
                else
                {
                    List<PaisesModel> paises = paisesDal.ObtenerPaises(usuario.usuario_id);

                    return paises;
                }

            }
            else
            {
                return null;
            }

        }
        public List<PaisesModelCiudadesOut>? ObtenerTodosPaisesByUsuarios()
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            CiudadesDal ciudadesDal = new CiudadesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            List<UsuarioModels> listUsuarios = loginBo.ObtenerTodosUsuarios();
            PaisesListUsuarioId paises = new PaisesListUsuarioId();
            List<List<PaisesModelCiudades>> list = new List<List<PaisesModelCiudades>>();
            List<PaisesModelCiudadesOut> listaSalida = new List<PaisesModelCiudadesOut>();
            if (listUsuarios != null)
            {
                foreach (UsuarioModels usuario in listUsuarios)
                {
                    List<PaisesModelCiudades> listPaisesUsuarios = paisesDal.ObtenerTodosPaisesByUsuarios(usuario.usuario_id);
                    if (listPaisesUsuarios != null && listPaisesUsuarios.Count > 0)
                    {
                        foreach (PaisesModelCiudades paisUsuario in listPaisesUsuarios)
                        {
                            List<CiudadesModel> ciudades = ciudadesDal.ObtenerCiudadesByPaises(paisUsuario.pais_id);
                            if (ciudades != null && ciudades.Count > 0)
                            {
                                paisUsuario.listCiudades = ciudades;
                                paisUsuario.listCiudadesSerialize = JsonConvert.SerializeObject(ciudades);
                            }
                        }
                        PaisesModelCiudadesOut objectSalida = new PaisesModelCiudadesOut();
                        objectSalida.usuario_id = usuario.usuario_id;
                        objectSalida.correo = usuario.correo;
                        objectSalida.nombre = usuario.nombre_completo;
                        objectSalida.listPaises = listPaisesUsuarios;
                        objectSalida.listPaisesSerialize = JsonConvert.SerializeObject(listPaisesUsuarios);
                        listaSalida.Add(objectSalida);
                    }



                }
                return listaSalida;
            }
            else
            {
                return null;
            }

        }
        public List<PaisesModel>? ImportarExcel(ExcelDataRequest request)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(request.usuario!);

            if (usuario != null)
            {
                LecturaExcelPais(usuario, request.base64string!);
                List<PaisesModel> paises = paisesDal.ObtenerPaises(usuario.usuario_id);
                return paises;

            }
            else
            {
                return null;
            }

        }

        private void LecturaExcelPais(UsuarioModels usuario, string file)
        {
            List<PaisesModel> paises = new();
            PaisesDal paisesDal = new PaisesDal(_config);
            try
            {
                DataTable dataExcel = utils.ConvertExcel(file);
                for (int i = 1; i <= dataExcel.Rows.Count - 1; i++)
                {
                    PaisesModel pais = new();
                    pais.nombre_pais = dataExcel.Rows[i][1].ToString();
                    pais.usuario_id = usuario.usuario_id;
                    pais.capital = dataExcel.Rows[i][2].ToString();
                    pais.region = dataExcel.Rows[i][3].ToString();
                    pais.poblacion = dataExcel.Rows[i][4].ToString();
                    paises.Add(pais);
                }
                foreach (PaisesModel pais in paises)
                {
                    paisesDal.InsertarPaises(pais);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<PaisesModel>? IngresarPais(PaisesModel paisRequest)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(paisRequest.usuario!);

            if (usuario != null)
            {
                paisRequest.usuario_id = usuario.usuario_id;
                paisesDal.InsertarPaises(paisRequest);
                List<PaisesModel> paises = paisesDal.ObtenerPaises(usuario.usuario_id);

                return paises;
            }
            else
            {
                return null;
            }

        }
        public List<PaisesModel>? ModificarPais(PaisesModel paisRequest)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(paisRequest.usuario!);

            if (usuario != null)
            {
                paisRequest.usuario_id = usuario.usuario_id;
                paisesDal.ModificarPais(paisRequest);
                List<PaisesModel> paises = paisesDal.ObtenerPaises(usuario.usuario_id);

                return paises;
            }
            else
            {
                return null;
            }

        }
        public List<PaisesModel>? EliminarPais(PaisesModel paisRequest)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(paisRequest.usuario!);

            if (usuario != null)
            {
                paisRequest.usuario_id = usuario.usuario_id;
                paisesDal.EliminarPais(paisRequest.pais_id);
                List<PaisesModel> paises = paisesDal.ObtenerPaises(usuario.usuario_id);

                return paises;
            }
            else
            {
                return null;
            }

        }
        public List<PaisesModel>? ObtenerPaisesPorFechas(UsuarioRequest request)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(request.usuario!);

            if (usuario != null)
            {
                List<PaisesModel> paises = paisesDal.ObtenerPaisesPorFecha(request.fecha_desde, request.fecha_hasta, usuario.usuario_id);

                return paises;
            }
            else
            {
                return null;
            }

        }
        public List<PaisesModel>? GetExcelPaises(UsuarioRequest request)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(request.usuario!);

            if (usuario != null)
            {
                List<PaisesModel> paises = paisesDal.ObtenerPaises(usuario.usuario_id);

                return paises;
            }
            else
            {
                return null;
            }

        }

    }
}
