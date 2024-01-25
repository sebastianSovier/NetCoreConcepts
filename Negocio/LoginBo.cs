using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using static NetCoreConcepts.Models.LoginModels;

namespace Negocio
{
    public class LoginBo
    {
        private readonly IConfiguration _config;

        public LoginBo(IConfiguration config)
        {
            _config = config;

        }


        public LoginBo() { }


        public UsuarioModels ObtenerUsuario(LoginRequest request)
        {
            try
            {
                UsuarioModels usuario = new UsuarioModels();
                UsuarioDal usuarioDal = new UsuarioDal(_config);

                usuario = usuarioDal.ObtenerUsuario(request.Username!);

                return usuario;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void CrearUsuario(UsuarioModels usuarioRequest)
        {
            UsuarioDal usuarioDal = new UsuarioDal(_config);

            usuarioDal.CrearUsuario(usuarioRequest);

            
        }
    }
}