using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar : IRequest<UsuarioData> {}

        public class Manejador : IRequestHandler<Ejecutar, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager1;
            private readonly IJwtGenerador jwtGenerador1;
            private readonly IUsuarioSesion usuarioSesion1;

            public Manejador(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion){
                userManager1 = userManager;
                jwtGenerador1 = jwtGenerador;
                usuarioSesion1 = usuarioSesion;
            }
            
            public async Task<UsuarioData> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var usuario = await userManager1.FindByNameAsync(usuarioSesion1.ObtenerUsuarioSesion());
                return new UsuarioData {
                    NombreCompleto = usuario.NombreCompleto,
                    Username = usuario.UserName,
                    Token = jwtGenerador1.CrearToken(usuario),
                    Imagen = null,
                    Email = usuario.Email
                };
            }
        }
    }
}