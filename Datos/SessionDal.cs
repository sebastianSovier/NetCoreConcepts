using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;

namespace Datos
{
    public class SessionDal
    {
        private readonly IConfiguration _config;
        MySqlConexion? mysql = null;
        UtilidadesApiss utils = new UtilidadesApiss();

        public SessionDal(IConfiguration config)
        {
            _config = config;
            mysql = new MySqlConexion(_config);
        }

        public void CrearSessionUser(SessionModels usuarioRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "INSERT INTO `bdpaises`.`Session` (`usuario_id`, `user_activo`) VALUES (?usuario_id, ?user_activo);";

                cmd.Parameters.Add("?usuario_id", MySqlDbType.VarChar).Value = usuarioRequest.usuario_id;
                cmd.Parameters.Add("?user_activo", MySqlDbType.VarChar).Value = "ACTIVO";

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
        public void UpdateSessionUser(SessionModels usuarioRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "UPDATE `bdpaises`.`Session` set user_activo = ?user_activo where usuario_id = ?usuario_id;";

                cmd.Parameters.Add("?user_activo", MySqlDbType.VarChar).Value = usuarioRequest.user_activo;
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
        public SessionModels ObtenerSessionByUser(SessionModels usuarioRequest)
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {

                SessionModels usuario = new SessionModels();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = $"select session_id,usuario_id,user_activo,fecha_actividad from Session where usuario_id = ?usuario_id order by session_id;";
                cmd.Parameters.Add("?usuario_id", MySqlDbType.VarChar).Value = usuarioRequest.usuario_id;
                using MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usuario.session_id = Convert.ToInt64(reader["session_id"]);
                    usuario.usuario_id = Convert.ToInt64(reader["usuario_id"]);
                    usuario.user_activo = reader["user_activo"].ToString();
                    usuario.fecha_actividad = reader["fecha_actividad"].ToString();
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
        public List<SessionModels> ObtenerSessionesUsuarios()
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                List<SessionModels> listUsuarios = new List<SessionModels>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = $"select session_id,usuario_id,user_activo,fecha_actividad from Session where user_activo = 'ACTIVO' order by fecha_actividad;";
                using MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    SessionModels Usuario = new SessionModels();
                    Usuario.usuario_id = Convert.ToInt64(reader["usuario_id"]);
                    Usuario.fecha_actividad = reader["fecha_actividad"].ToString();
                    Usuario.session_id = Convert.ToInt64(reader["session_id"]);
                    Usuario.user_activo = reader["user_activo"].ToString();
                    listUsuarios.Add(Usuario);


                }

                return listUsuarios;
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
