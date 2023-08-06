using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using MediatR;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Eliminar
    {
        
        public class Ejecuta : IRequest{
             public Guid CursoId { get; set;}
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CursosOnlineContext context1;
            public Manejador(CursosOnlineContext context) {
            context1 = context;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var instructoresBD = context1.CursoInstructor.Where(x => x.CursoId == request.CursoId);
                foreach(var instructor in instructoresBD ){
                    context1.CursoInstructor.Remove(instructor);
                }

                var comentariosDB = context1.Comentario.Where(x => x.CursoId == request.CursoId).ToList();
                foreach(var coment in comentariosDB){
                    context1.Comentario.Remove(coment);
                }

                var precioDB = context1.Precio.Where(x => x.CursoId == request.CursoId).FirstOrDefault();
                if(precioDB != null){
                    context1.Precio.Remove(precioDB);
                }

                var curso = await context1.Curso.FindAsync(request.CursoId);
                if(curso==null){
                    //throw new Exception("No se puede eliminar curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el curso"});
                }
                context1.Remove(curso);
                var resultado = await context1.SaveChangesAsync();
                if(resultado>0){
                    return Unit.Value;
                }
                throw new Exception("No se pudieron guardar los cambios");
            }
        }
    }
}