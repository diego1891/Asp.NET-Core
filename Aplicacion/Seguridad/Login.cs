using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Contratos;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioData> {
            public string Email { get; set; } 
            public string Password { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor( x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();    
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly UserManager<Usuario> userManager1;
            private readonly SignInManager<Usuario> signInManager1;
            private readonly IJwtGenerador jwtGenerador1;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador){
                userManager1 = userManager;
                signInManager1 = signInManager;
                jwtGenerador1 = jwtGenerador;
            }

            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var usuario = await userManager1.FindByEmailAsync(request.Email);
                if(usuario == null){
                    throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);
                }
                var resultado = await signInManager1.CheckPasswordSignInAsync(usuario, request.Password, false);
                if(resultado.Succeeded){
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Token = jwtGenerador1.CrearToken(usuario),
                        Username = usuario.UserName,
                        Email = usuario.Email,
                        Imagen = null
                    };
                }

                throw new ManejadorExcepcion(HttpStatusCode.Unauthorized);

            }
        }
    }

}