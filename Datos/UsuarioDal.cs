using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;

namespace NetCoreConcepts.Dal
{
    public class UsuarioDal
    {
        private readonly IConfiguration _config;
        MySqlConexion? mysql = null;
        UtilidadesApiss utils = new UtilidadesApiss();

        public UsuarioDal(IConfiguration config)
        {
            _config = config;
            mysql = new MySqlConexion(_config);
        }

        public List<UsuarioModels> ObtenerUsuarios()
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                List<UsuarioModels> listUsuarios = new List<UsuarioModels>();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = $"select usuario_id,usuario,nombre_completo,correo,fecha_registro from Usuarios order by usuario_id;";
                using MySqlDataReader reader = cmd.ExecuteReader();


                while (reader.Read())
                {
                    UsuarioModels Usuario = new UsuarioModels();
                    Usuario.usuario_id = Convert.ToInt32(reader["usuario_id"]);
                    Usuario.usuario = reader["usuario"].ToString();
                    Usuario.nombre_completo = reader["nombre_completo"].ToString();
                    Usuario.correo = reader["correo"].ToString();
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
        public UsuarioModels ObtenerUsuario(string usuarioRequest)
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                UsuarioModels usuario = new UsuarioModels();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = $"select usuario_id,usuario,contrasena,nombre_completo,correo,fecha_registro from Usuarios where usuario = ?usuario order by usuario_id;";
                cmd.Parameters.Add("?usuario", MySqlDbType.VarChar).Value = usuarioRequest;
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UsuarioModels Usuario = new UsuarioModels();
                    Usuario.usuario_id = Convert.ToInt32(reader["usuario_id"]);
                    Usuario.contrasena = reader["contrasena"].ToString();
                    Usuario.usuario = reader["usuario"].ToString();
                    Usuario.nombre_completo = reader["nombre_completo"].ToString();
                    Usuario.correo = reader["correo"].ToString();
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
        public List<UsuarioModels> ObtenerTodosUsuarios()
        {
            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                List<UsuarioModels> list = new();
                UsuarioModels usuario = new UsuarioModels();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = $"select usuario_id,usuario,contrasena,nombre_completo,correo,fecha_registro from Usuarios order by usuario_id;";
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UsuarioModels Usuario = new UsuarioModels();
                    Usuario.usuario_id = Convert.ToInt32(reader["usuario_id"]);
                    Usuario.usuario = reader["usuario"].ToString();
                    Usuario.nombre_completo = reader["nombre_completo"].ToString();
                    Usuario.correo = reader["correo"].ToString();
                    usuario = Usuario;
                    list.Add(usuario);

                }

                return list;
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

        public void CrearUsuario(UsuarioModels usuarioRequest)
        {

            using MySqlConnection conexion = mysql!.getConexion("bdpaises1");
            try
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "INSERT INTO `bdpaises`.`Usuarios` (`usuario`, `contrasena`, `nombre_completo`, `correo`) VALUES (?usuario, ?contrasena, ?nombre_completo, ?correo);";

                cmd.Parameters.Add("?usuario", MySqlDbType.VarChar).Value = usuarioRequest.usuario;
                cmd.Parameters.Add("?contrasena", MySqlDbType.VarChar).Value = usuarioRequest.contrasena;
                cmd.Parameters.Add("?nombre_completo", MySqlDbType.VarChar).Value = usuarioRequest.nombre_completo;
                cmd.Parameters.Add("?correo", MySqlDbType.VarChar).Value = usuarioRequest.correo;

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


