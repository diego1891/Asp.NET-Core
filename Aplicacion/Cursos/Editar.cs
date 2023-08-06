using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using Dominio;
using FluentValidation;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Editar
    {
        public class Ejecuta : IRequest{
            public Guid CursoId { get; set;}
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public List<Guid> ListaInstructor { get; set; }
            public decimal? Precio { get; set; }
            public decimal? Promocion { get; set; }
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
                var curso = await context1.Curso.FindAsync(request.CursoId);
                if(curso==null){
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el curso"});
                }

                curso.Titulo = request.Titulo ?? curso.Titulo;
                curso.Descripcion = request.Descripcion ?? curso.Descripcion;
                curso.FechaPublicacion = request.FechaPublicacion ?? curso.FechaPublicacion;
                
                /*Actualizar precio Curso*/

                var precioEntidad = context1.Precio.Where(x => x.CursoId == curso.CursoId).FirstOrDefault();
                if(precioEntidad!=null){
                    precioEntidad.Promocion = request.Promocion ?? precioEntidad.Promocion;
                    precioEntidad.PrecioActual = request.Precio ?? precioEntidad.PrecioActual;
                }
                else{
                    precioEntidad = new Precio{
                        PrecioId = Guid.NewGuid(),
                        PrecioActual = request.Precio ?? 0,
                        Promocion = request.Promocion ?? 0,
                        CursoId = curso.CursoId
                    };
                    await context1.Precio.AddAsync(precioEntidad);
                }

                if(request.ListaInstructor != null){
                    if(request.ListaInstructor.Count > 0){
                        /*Eliminar los instructores actuales en la bd*/
                        var instructoresBD = context1.CursoInstructor.Where(x => x.CursoId == request.CursoId).ToList();
                        foreach(var instructorEliminar in instructoresBD){
                            context1.CursoInstructor.Remove(instructorEliminar);
                        }
                        /*Agregar los nuevos que envía el cliente*/
                        foreach(var id in request.ListaInstructor){
                            var nuevoInstructor = new CursoInstructor{
                                CursoId = request.CursoId,
                                InstructorId = id
                            };
                            context1.CursoInstructor.Add(nuevoInstructor);
                        }
                    }

                }
                
                var resultado = await context1.SaveChangesAsync();
                
                if(resultado>0){
                    return Unit.Value;
                }
                throw new Exception("No se guardaron los cambios en el curso");
            }
        }

    }
}