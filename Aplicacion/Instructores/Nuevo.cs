using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Nuevo
    {

        public class Ejecuta : IRequest {
            public string Nombre { get; set; }
            public string Apellidos { get; set; }
            public string Titulo { get; set; }
        }

        /*clase para validar q los datos no esten vacios*/
        public class EjecutaValida : AbstractValidator<Ejecuta>{
            public EjecutaValida(){
                RuleFor(x => x.Nombre).NotEmpty(); 
                RuleFor(x => x.Apellidos).NotEmpty(); 
                RuleFor(x => x.Titulo).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructorRepository1;
            public Manejador(IInstructor instructorRepository){
                instructorRepository1 = instructorRepository;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await instructorRepository1.Nuevo(request.Nombre, request.Apellidos, request.Titulo);
                if(resultado > 0){
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el instructor");
            }
        }
    }
}