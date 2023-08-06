using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.ManejadorError;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class ConsultaId
    {
        public class CursoUnico : IRequest<CursoDto>{
            public Guid Id { get; set;}
        }

        public class Manejador : IRequestHandler<CursoUnico, CursoDto>
        {
            private readonly CursosOnlineContext context1;
            private readonly IMapper mapper1;
            public Manejador(CursosOnlineContext context, IMapper mapper)
            {
                context1 = context;
                mapper1 = mapper;
            }

            public async Task<CursoDto> Handle(CursoUnico request, CancellationToken cancellationToken)
            {
                var curso = await context1.Curso
                .Include(x => x.ComentarioLista)
                .Include(x => x.PrecioPromocion)
                .Include(x=> x.InstructorLink)
                .ThenInclude(y => y.Instructor)
                .FirstOrDefaultAsync(a => a.CursoId == request.Id);

                if(curso==null){
                    //throw new Exception("No se puede eliminar curso");
                    throw new ManejadorExcepcion(HttpStatusCode.NotFound, new {mensaje = "No se encontro el curso"});
                }

                var cursoDto = mapper1.Map<Curso, CursoDto>(curso);
                return  cursoDto;
            }
        }
    }
}