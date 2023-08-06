using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Nuevo
    {
        public class Ejecuta : IRequest {
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal Precio { get; set; }
            public decimal Promocion { get; set;}
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.Descripcion).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
            }
        }


        public class Manejador : IRequestHandler<Ejecuta>
        {

            private readonly CursosOnlineContext context1;
            public Manejador(CursosOnlineContext context){
                context1 = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Guid cursoId1 = Guid.NewGuid();
                var curso = new Curso {
                    CursoId = cursoId1,
                    Titulo = request.Titulo,
                    Descripcion = request.Descripcion,
                    FechaPublicacion = request.FechaPublicacion,
                    FechaCreacion = DateTime.UtcNow
                };

                context1.Curso.Add(curso);

                if(request.ListaInstructor!=null){
                    foreach(var id in request.ListaInstructor){
                        var cursoInstructor = new CursoInstructor{
                            CursoId = cursoId1,
                            InstructorId = id
                        };
                        context1.CursoInstructor.Add(cursoInstructor);
                    }
                }

                /*Agregar Precio al curso */
                var precioEntidad = new Precio{
                    CursoId = cursoId1,
                    PrecioActual = request.Precio,
                    Promocion = request.Promocion,
                    PrecioId = Guid.NewGuid()
                };

                context1.Precio.Add(precioEntidad);
                var valor = await context1.SaveChangesAsync();
                if(valor>0){
                    return Unit.Value;
                }
                throw new Exception("No se pudo insertar el curso ");
            }
        }
    }
}