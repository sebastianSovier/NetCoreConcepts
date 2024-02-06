using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;
using System.Data;

namespace NetCoreConcepts.Dal
{
    public class PaisesDal
    {
        private readonly IConfiguration _config;
        MySqlConexion? mysql = null;
        UtilidadesApiss utils = new UtilidadesApiss();

        public PaisesDal(IConfiguration config)
        {
            _config = config;
            mysql = new MySqlConexion(_config);
        }

        public List<PaisesModel> ObtenerPaises(Int64 usuario_id)
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {
                List<PaisesModel> listPaises = new List<PaisesModel>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "p_listar_paises";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_usuario_id", usuario_id);
                cmd.Parameters["@p_usuario_id"].Direction = ParameterDirection.Input;

                using MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    PaisesModel paises = new PaisesModel();
                    paises.pais_id = Convert.ToInt32(reader["pais_id"]);
                    paises.nombre_pais = reader["nombre_pais"].ToString();
                    paises.capital = reader["capital"].ToString();
                    paises.region = reader["region"].ToString();
                    paises.poblacion = reader["poblacion"].ToString();
                    listPaises.Add(paises);

                }
                return listPaises;
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
        public List<PaisesModel> ObtenerPaisesPorFecha(string? fecha_desde, string? fecha_hasta, Int64 usuario_id)
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {
                List<PaisesModel> listPaises = new List<PaisesModel>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "p_buscar_paises";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_fecha_desde", fecha_desde);
                cmd.Parameters.AddWithValue("@p_fecha_hasta", fecha_hasta);
                cmd.Parameters.AddWithValue("@p_usuario_id", usuario_id);

                cmd.Parameters["@p_fecha_desde"].Direction = ParameterDirection.Input;
                cmd.Parameters["@p_fecha_hasta"].Direction = ParameterDirection.Input;
                cmd.Parameters["@p_usuario_id"].Direction = ParameterDirection.Input;

                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PaisesModel paises = new PaisesModel();
                    paises.pais_id = Convert.ToInt32(reader["pais_id"]);
                    paises.nombre_pais = reader["nombre_pais"].ToString();
                    paises.capital = reader["capital"].ToString();
                    paises.region = reader["region"].ToString();
                    paises.poblacion = reader["poblacion"].ToString();
                    listPaises.Add(paises);

                }
                conexion.Close();
                return listPaises;
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


        public void InsertarPaises(PaisesModel paisRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "INSERT INTO `bdpaises`.`Paises` (`nombre_pais`, `capital`, `region`, `poblacion`,`usuario_id`) VALUES (?nombre_pais, ?capital, ?region, ?poblacion,?usuario_id);";

                cmd.Parameters.Add("?nombre_pais", MySqlDbType.VarChar).Value = paisRequest.nombre_pais;
                cmd.Parameters.Add("?capital", MySqlDbType.VarChar).Value = paisRequest.capital;
                cmd.Parameters.Add("?region", MySqlDbType.VarChar).Value = paisRequest.region;
                cmd.Parameters.Add("?poblacion", MySqlDbType.VarChar).Value = paisRequest.poblacion;
                cmd.Parameters.Add("?usuario_id", MySqlDbType.VarChar).Value = paisRequest.usuario_id;
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


        public void ModificarPais(PaisesModel paisRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "UPDATE `bdpaises`.`Paises` set nombre_pais = ?nombre_pais, capital = ?capital, region = ?region, poblacion = ?poblacion where pais_id = ?pais_id";
                cmd.Parameters.Add("?pais_id", MySqlDbType.VarChar).Value = paisRequest.pais_id;
                cmd.Parameters.Add("?nombre_pais", MySqlDbType.VarChar).Value = paisRequest.nombre_pais;
                cmd.Parameters.Add("?capital", MySqlDbType.VarChar).Value = paisRequest.capital;
                cmd.Parameters.Add("?region", MySqlDbType.VarChar).Value = paisRequest.region;
                cmd.Parameters.Add("?poblacion", MySqlDbType.VarChar).Value = paisRequest.poblacion;

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

        public void EliminarPais(Int64 pais_id)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            try
            {

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "Delete from `bdpaises`.`Paises` where pais_id = ?pais_id ";

                cmd.Parameters.Add("?pais_id", MySqlDbType.VarChar).Value = pais_id;

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
