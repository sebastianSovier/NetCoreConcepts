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
                cmd.CommandText = $"select usuario_id,usuario,nombre_completo,correo,fecha_registro from Usuario order by usuario_id;";
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
                cmd.CommandText = $"select usuario_id,usuario,nombre_completo,correo,fecha_registro from Usuario where usuario = ?usuario order by usuario_id;";
                cmd.Parameters.Add("?usuario", MySqlDbType.VarChar).Value = usuarioRequest;
                using MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    UsuarioModels Usuario = new UsuarioModels();
                    Usuario.usuario_id = Convert.ToInt32(reader["usuario_id"]);
                    // Usuario.contrasena = reader["contrasena"].ToString();
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
                cmd.CommandText = $"select usuario_id,usuario,nombre_completo,correo,fecha_registro from Usuario order by usuario_id;";
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
                cmd.CommandText = "INSERT INTO `bdpaises`.`Usuario` (`usuario`, `nombre_completo`, `correo`) VALUES (?usuario, ?nombre_completo, ?correo);";

                cmd.Parameters.Add("?usuario", MySqlDbType.VarChar).Value = usuarioRequest.usuario;
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

    }
}


