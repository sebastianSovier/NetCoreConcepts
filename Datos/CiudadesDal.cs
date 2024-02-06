using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;

namespace Datos
{
    public class CiudadesDal
    {
        private readonly IConfiguration _config;
        MySqlConexion? mysql = null;
        UtilidadesApiss utils = new UtilidadesApiss();

        public CiudadesDal(IConfiguration config)
        {
            _config = config;
            mysql = new MySqlConexion(_config);

        }

        public List<CiudadesModel> ObtenerCiudades(Int64 pais_id)
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {
                List<CiudadesModel> listaCiudades = new List<CiudadesModel>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = $"select ciudad_id,pais_id,nombre_ciudad,poblacion,region,fecha_registro,latitud,longitud from Ciudades where pais_id = ?pais_id order by pais_id;";
                cmd.Parameters.Add("?pais_id", MySqlDbType.Int64).Value = pais_id;


                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    CiudadesModel paises = new CiudadesModel();
                    paises.ciudad_id = Convert.ToInt32(reader["ciudad_id"]);
                    paises.pais_id = Convert.ToInt32(reader["pais_id"]);
                    paises.nombre_ciudad = reader["nombre_ciudad"].ToString();
                    paises.region = reader["region"].ToString();
                    paises.poblacion = reader["poblacion"].ToString();
                    paises.latitud = reader["latitud"].ToString();
                    paises.longitud = reader["longitud"].ToString();
                    listaCiudades.Add(paises);

                }
                return listaCiudades;
            }
            catch (Exception ex)
            {

                utils.createlogFile(ex.Message); throw;
            }
            finally
            {
                conexion.Close();
            }


        }
        public void InsertarCiudad(CiudadesModel ciudadRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "INSERT INTO `bdpaises`.`Ciudades` (`pais_id`, `nombre_ciudad`, `poblacion`, `region`, `latitud`, `longitud`) VALUES (?pais_id, ?nombre_ciudad, ?poblacion, ?region, ?latitud, ?longitud);";

                cmd.Parameters.Add("?pais_id", MySqlDbType.Int64).Value = ciudadRequest.pais_id;
                cmd.Parameters.Add("?nombre_ciudad", MySqlDbType.VarChar).Value = ciudadRequest.nombre_ciudad;
                cmd.Parameters.Add("?poblacion", MySqlDbType.VarChar).Value = ciudadRequest.poblacion;
                cmd.Parameters.Add("?region", MySqlDbType.VarChar).Value = ciudadRequest.region;
                cmd.Parameters.Add("?latitud", MySqlDbType.VarChar).Value = ciudadRequest.latitud;
                cmd.Parameters.Add("?longitud", MySqlDbType.VarChar).Value = ciudadRequest.longitud;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                utils.createlogFile(ex.Message); throw;
            }
            finally
            {
                conexion.Close();
            }

        }
        public void ModificarCiudad(CiudadesModel ciudadRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "UPDATE `bdpaises`.`Ciudades` set nombre_ciudad = ?nombre_ciudad, region = ?region, poblacion = ?poblacion, latitud = ?latitud, longitud = ?longitud where ciudad_id = ?ciudad_id";

                cmd.Parameters.Add("?nombre_ciudad", MySqlDbType.VarChar).Value = ciudadRequest.nombre_ciudad;
                cmd.Parameters.Add("?region", MySqlDbType.VarChar).Value = ciudadRequest.region;
                cmd.Parameters.Add("?poblacion", MySqlDbType.VarChar).Value = ciudadRequest.poblacion;
                cmd.Parameters.Add("?ciudad_id", MySqlDbType.Int64).Value = ciudadRequest.ciudad_id;
                cmd.Parameters.Add("?latitud", MySqlDbType.VarChar).Value = ciudadRequest.latitud;
                cmd.Parameters.Add("?longitud", MySqlDbType.VarChar).Value = ciudadRequest.longitud;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                utils.createlogFile(ex.Message); throw;
            }
            finally
            {
                conexion.Close();
            }

        }
        public void EliminarCiudad(Int64 ciudad_id)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "Delete from `bdpaises`.`Ciudades` where ciudad_id = ?ciudad_id ";

                cmd.Parameters.Add("?ciudad_id", MySqlDbType.VarChar).Value = ciudad_id;

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                utils.createlogFile(ex.Message); throw;
            }
            finally
            {
                conexion.Close();
            }

        }
    }
}
