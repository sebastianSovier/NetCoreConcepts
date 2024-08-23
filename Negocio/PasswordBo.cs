using Datos;
using Microsoft.Extensions.Configuration;
using NetCoreConcepts.Models;
using NetCoreConcepts.UtilidadesApi;

namespace Negocio
{
    public class PasswordBo
    {
        private readonly IConfiguration _config;
        private UtilidadesApiss util = new UtilidadesApiss();
        public PasswordBo(IConfiguration config)
        {
            _config = config;

        }


        public PasswordBo() { }


        public PasswordModels ObtenerPassword(string usuario_id)
        {
            PasswordModels password = new PasswordModels();
            PasswordDal passwordDal = new PasswordDal(_config);
            password = passwordDal.ObtenerPassword(usuario_id);
            return password;

        }
        public PasswordModels GenerarCodigoRecuperacion(PasswordModels passwordRequest)
        {
            PasswordDal passwordDal = new PasswordDal(_config);
            PasswordModels password = new();
            passwordRequest.cod_recover_password = util.generateRandomNumber();
            passwordDal.CodigoRecuperacion(passwordRequest);
            password.cod_recover_password = passwordRequest.cod_recover_password;
            return password;

        }
        public void CambioPassword(PasswordModels passwordRequest)
        {
            PasswordDal passwordDal = new PasswordDal(_config);

            passwordDal.CambioPassword(passwordRequest);


        }
        public void CrearPassword(PasswordModels passwordRequest)
        {
            PasswordDal passwordDal = new PasswordDal(_config);

            passwordDal.CrearPassword(passwordRequest);


        }

    }
}
