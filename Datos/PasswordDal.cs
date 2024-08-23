using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;

namespace Datos
{
    public class PasswordDal
    {
        private readonly IConfiguration _config;
        MySqlConexion? mysql = null;
        UtilidadesApiss utils = new UtilidadesApiss();

        public PasswordDal(IConfiguration config)
        {
            _config = config;
            mysql = new MySqlConexion(_config);
        }
        public PasswordModels ObtenerPassword(string usuarioRequest)
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                PasswordModels usuario = new PasswordModels();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = $"select password_id,usuario_id,password,fecha_registro,cod_recover_password from Password where usuario_id = ?usuario_id order by usuario_id;";
                cmd.Parameters.Add("?usuario_id", MySqlDbType.VarChar).Value = usuarioRequest;
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PasswordModels Usuario = new PasswordModels();
                    Usuario.usuario_id = Convert.ToInt32(reader["usuario_id"]);
                    Usuario.password = reader["password"].ToString();
                    Usuario.fecha_registro = reader["fecha_registro"].ToString();
                    Usuario.cod_recover_password = reader["cod_recover_password"].ToString();
                    usuario = Usuario;

                }

                return usuario;
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

        public void CrearPassword(PasswordModels usuarioRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "INSERT INTO `bdpaises`.`Password` (`usuario_id`, `password`) VALUES (?usuario_id, ?password);";

                cmd.Parameters.Add("?usuario_id", MySqlDbType.VarChar).Value = usuarioRequest.usuario_id;
                cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = usuarioRequest.password;

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
        public void CodigoRecuperacion(PasswordModels usuarioRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "UPDATE `bdpaises`.`Password` set cod_recover_password = ?cod_recover_password where usuario_id = ?usuario_id;";

                cmd.Parameters.Add("?cod_recover_password", MySqlDbType.VarChar).Value = usuarioRequest.cod_recover_password;
                cmd.Parameters.Add("?usuario_id", MySqlDbType.VarChar).Value = usuarioRequest.usuario_id;
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
        public void CambioPassword(PasswordModels usuarioRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "UPDATE `bdpaises`.`Password` set password = ?password where usuario_id = ?usuario_id;";

                cmd.Parameters.Add("?usuario_id", MySqlDbType.VarChar).Value = usuarioRequest.usuario_id;
                cmd.Parameters.Add("?password", MySqlDbType.VarChar).Value = usuarioRequest.password;

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
