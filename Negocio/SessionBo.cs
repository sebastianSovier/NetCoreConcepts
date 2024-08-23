using Datos;
using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;

namespace Negocio
{
    public class SessionBo
    {
        private readonly IConfiguration _config;
        private UtilidadesApiss util = new UtilidadesApiss();
        public SessionBo(IConfiguration config)
        {
            _config = config;

        }


        public SessionBo() { }


        public void CrearSession(SessionModels usuarioRequest)
        {
            SessionDal sessionDal = new SessionDal(_config);

            sessionDal.CrearSessionUser(usuarioRequest);


        }
        public SessionModels ObtenerSessionUsuario(SessionModels sessionUser)
        {
            SessionModels session = new SessionModels();
            SessionDal sessionDal = new SessionDal(_config);
            session = sessionDal.ObtenerSessionByUser(sessionUser);
            return session;

        }
        public void ObtenerSessionesUsuariosInactivos()
        {
            List<SessionModels> sessiones = new List<SessionModels>();
            SessionDal sessionDal = new SessionDal(_config);
            sessiones = sessionDal.ObtenerSessionesUsuarios();

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
            SessionDal sessionDal = new SessionDal(_config);
            sessionDal.UpdateSessionUser(sessionUser);

        }
        public void UpdateSessionLogoutUser(SessionModels sessionUser)
        {
            SessionDal sessionDal = new SessionDal(_config);
            UsuarioDal usuarioDal = new UsuarioDal(_config);
            UsuarioModels usuario = usuarioDal.ObtenerUsuario(sessionUser.usuario!);
            sessionUser.usuario_id = usuario.usuario_id;
            SessionModels session = sessionDal.ObtenerSessionByUser(sessionUser);
            sessionUser.session_id = session.session_id;

            sessionDal.UpdateSessionUser(sessionUser);

        }

    }
}
