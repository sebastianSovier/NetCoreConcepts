using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;

namespace Negocio
{
    public class LoginBo
    {
        private readonly IConfiguration _config;
        private UtilidadesApiss util = new UtilidadesApiss();
        public LoginBo(IConfiguration config)
        {
            _config = config;

        }


        public LoginBo() { }


        public UsuarioModels ObtenerUsuario(string userName)
        {
            UsuarioModels usuario = new UsuarioModels();
            UsuarioDal usuarioDal = new UsuarioDal(_config);
            usuario = usuarioDal.ObtenerUsuario(util.CleanObject(userName));
            return usuario;

        }
        public List<UsuarioModels> ObtenerTodosUsuarios()
        {
            List<UsuarioModels> usuario = new();
            UsuarioDal usuarioDal = new UsuarioDal(_config);
            usuario = usuarioDal.ObtenerTodosUsuarios();
            return usuario;

        }
        public void CrearUsuario(UsuarioModels usuarioRequest)
        {
            UsuarioDal usuarioDal = new UsuarioDal(_config);

            usuarioDal.CrearUsuario(util.CleanObject(usuarioRequest));


        }

    }
}