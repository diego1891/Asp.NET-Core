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
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioData>{
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Username { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>{
            public EjecutaValidador(){
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioData>
        {
            private readonly CursosOnlineContext context1;
            private readonly UserManager<Usuario> userManager1;
            private readonly IJwtGenerador jwtGenerador1;
            public Manejador(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador){
                context1 = context;
                userManager1 = userManager;
                jwtGenerador1 = jwtGenerador;
            }
            public async Task<UsuarioData> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var existe = await context1.Users.Where(x => x.Email == request.Email).AnyAsync();
                if (existe){
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "Existe ya un usuario con este email"});
                }

                var existeUserName = await context1.Users.Where(x => x.UserName == request.Username).AnyAsync();
                if(existeUserName){
                    throw new ManejadorExcepcion(HttpStatusCode.BadRequest, new {mensaje = "Exista ya un usuario con este username"});
                }

                var usuario = new Usuario {
                    NombreCompleto = request.Nombre + " " + request.Apellidos,
                    Email = request.Email,
                    UserName = request.Username
                };

                var resultado = await userManager1.CreateAsync(usuario,request.Password);
                if(resultado.Succeeded)
                    return new UsuarioData{
                        NombreCompleto = usuario.NombreCompleto,
                        Token = jwtGenerador1.CrearToken(usuario),
                        Username = usuario.UserName,
                        Email = usuario.Email
                    };

                throw new Exception("No se pudo agregar al nuevo usuario");
            }
        }
    }
}