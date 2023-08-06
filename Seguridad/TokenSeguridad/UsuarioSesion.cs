using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Microsoft.AspNetCore.Http;

namespace Seguridad.TokenSeguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {
        private readonly IHttpContextAccessor httpContextAccessor1;
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor){
            httpContextAccessor1 = httpContextAccessor;
        }
        public string ObtenerUsuarioSesion()
        {
            var userName = httpContextAccessor1.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return userName;
        }
    }
}