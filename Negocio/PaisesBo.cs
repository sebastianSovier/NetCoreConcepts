using Microsoft.Extensions.Configuration;
using Negocio;
using NetCoreConcepts.Dal;
using NetCoreConcepts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static NetCoreConcepts.Models.LoginModels;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace NetCoreConcepts.Bo
{
    public class PaisesBo
    {
        private readonly IConfiguration _config;
        public PaisesBo(IConfiguration config)
        {
            _config = config;

        }
        public List<PaisesModel>? ObtenerPaises(UsuarioRequest request)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(request.usuario!);

            if(usuario != null )
            {
                List<PaisesModel> paises = paisesDal.ObtenerPaises(usuario.usuario_id);

                return paises;
            }
            else
            {
                return null;
            }

        }
        public List<PaisesModel>? ObtenerPaisesPorFechas(UsuarioRequest request)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(request.usuario!);

            if (usuario != null)
            {
                List<PaisesModel> paises = paisesDal.ObtenerPaisesPorFecha(request.fecha_desde, request.fecha_hasta, usuario.usuario_id);

                return paises;
            }
            else
            {
                return null;
            }

        }
        public List<PaisesModel>? GetExcelPaises(UsuarioRequest request)
        {
            PaisesDal paisesDal = new PaisesDal(_config);
            LoginBo loginBo = new LoginBo(_config);

            UsuarioModels usuario = loginBo.ObtenerUsuario(request.usuario!);

            if (usuario != null)
            {
                List<PaisesModel> paises = paisesDal.ObtenerPaises(usuario.usuario_id);

                return paises;
            }
            else
            {
                return null;
            }

        }

    }
}
