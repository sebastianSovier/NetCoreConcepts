using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using NetCoreConcepts.Models;
using System.Data;

namespace NetCoreConcepts.Dal
{
    public class PaisesDal
    {
        private readonly IConfiguration _config;
        MySqlConexion? mysql = null;
        public PaisesDal(IConfiguration config)
        {
            _config = config;
            mysql = new MySqlConexion(_config);
        }

        public List<PaisesModel> ObtenerPaises(Int64 usuario_id)
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
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
            conexion.Close();
            return listPaises;
        }
        public List<PaisesModel> ObtenerPaisesPorFecha(string? fecha_desde, string? fecha_hasta, Int64 usuario_id)
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
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


        public void InsertarPaises(PaisesModel paisRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");

            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "INSERT INTO `bdPaises`.`Paises` (`nombre_pais`, `capital`, `region`, `poblacion`) VALUES (?nombre_pais, ?capital, ?region, ?poblacion);";

            cmd.Parameters.Add("?nombre_pais", MySqlDbType.VarChar).Value = paisRequest.nombre_pais;
            cmd.Parameters.Add("?capital", MySqlDbType.VarChar).Value = paisRequest.capital;
            cmd.Parameters.Add("?region", MySqlDbType.VarChar).Value = paisRequest.region;
            cmd.Parameters.Add("?poblacion", MySqlDbType.VarChar).Value = paisRequest.poblacion;

            cmd.ExecuteNonQuery();
            conexion.Close();
        }


        public void ModificarPais(PaisesModel paisRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            conexion.Open();
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "UPDATE `bdPaises`.`Paises` set nombre_pais = ?nombre_pais, capital = ?capital, region = ?region, poblacion = ?poblacion where pais_id = ?pais_id";
            cmd.Parameters.Add("?pais_id", MySqlDbType.VarChar).Value = paisRequest.pais_id;
            cmd.Parameters.Add("?nombre_pais", MySqlDbType.VarChar).Value = paisRequest.nombre_pais;
            cmd.Parameters.Add("?capital", MySqlDbType.VarChar).Value = paisRequest.capital;
            cmd.Parameters.Add("?region", MySqlDbType.VarChar).Value = paisRequest.region;
            cmd.Parameters.Add("?poblacion", MySqlDbType.VarChar).Value = paisRequest.poblacion;

            cmd.ExecuteNonQuery();
            conexion.Close();

        }

        public void EliminarPais(Int64 pais_id)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises");
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "Delete from `bdPaises`.`Paises` where pais_id = ?pais_id ";

            cmd.Parameters.Add("?pais_id", MySqlDbType.VarChar).Value = pais_id;

            cmd.ExecuteNonQuery();
            conexion.Close();

        }

    }
}
