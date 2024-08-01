using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Models;
using Negocio;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;

namespace Utilidades
{
    public class FilterSessionAttribute : ActionFilterAttribute
    {
        private readonly IConfiguration _config;
        private UtilidadesApiss utils = new UtilidadesApiss();
        public FilterSessionAttribute(IConfiguration config)
        {
            _config = config;

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Aquí puedes acceder a los parámetros de la acción
            IUsuarioValidation? parameter = context.ActionArguments
       .Values
       .OfType<IUsuarioValidation>()
       .FirstOrDefault();

            if (parameter == null || !IsValid(parameter.usuario!))
            {
                // Aquí puedes establecer una respuesta de error
                context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult("Sesion Expirada , Inicie Sesion nuevamente.");
            }

            base.OnActionExecuting(context);
        }

        private bool IsValid(string parameter)
        {
            LoginBo login = new LoginBo(_config);
            UsuarioModels userResp = login.ObtenerUsuario(parameter);
            SessionModels req = new SessionModels();
            req.usuario_id = userResp.usuario_id;
            SessionModels sessionResp = login.ObtenerSessionUsuario(req);

            if (utils.validTimeSession(sessionResp.fecha_actividad!))
            {
                sessionResp.user_activo = "INACTIVO";
                login.UpdateSessionUser(sessionResp);
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
