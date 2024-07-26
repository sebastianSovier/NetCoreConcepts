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
            usuario = usuarioDal.ObtenerUsuario(userName);
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

            usuarioDal.CrearUsuario(usuarioRequest);


        }
        public void CrearSession(SessionModels usuarioRequest)
        {
            UsuarioDal usuarioDal = new UsuarioDal(_config);

            usuarioDal.CrearSessionUser(usuarioRequest);


        }




        public SessionModels ObtenerSessionUsuario(SessionModels sessionUser)
        {
            SessionModels session = new SessionModels();
            UsuarioDal usuarioDal = new UsuarioDal(_config);
            session = usuarioDal.ObtenerSessionByUser(sessionUser);
            return session;

        }
        public void ObtenerSessionesUsuariosInactivos()
        {
            List<SessionModels> sessiones = new List<SessionModels>();
            UsuarioDal usuarioDal = new UsuarioDal(_config);
            sessiones = usuarioDal.ObtenerSessionesUsuarios();

            foreach (SessionModels sessionModel in sessiones)
            {

                if (util.validTimeSession(sessionModel.fecha_actividad!))
                {
                    sessionModel.user_activo = "INACTIVO";
                    UpdateSessionUser(sessionModel);
                }
            }

        }
        public void UpdateSessionUser(SessionModels sessionUser)
        {
            UsuarioDal usuarioDal = new UsuarioDal(_config);
            usuarioDal.UpdateSessionUser(sessionUser);

        }
        public void UpdateSessionLogoutUser(SessionModels sessionUser)
        {
            UsuarioDal usuarioDal = new UsuarioDal(_config);
            UsuarioModels usuario = usuarioDal.ObtenerUsuario(sessionUser.usuario!);
            sessionUser.usuario_id = usuario.usuario_id;
            SessionModels session = usuarioDal.ObtenerSessionByUser(sessionUser);
            sessionUser.session_id = session.session_id;

            usuarioDal.UpdateSessionUser(sessionUser);

        }
    }
}