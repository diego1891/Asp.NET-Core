using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Dominio;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Cursos
{
    public class Consulta
    {
        public class ListaCursos : IRequest<List<CursoDto>>{}

        public class Manejador : IRequestHandler<ListaCursos, List<CursoDto>>
        {
            private readonly CursosOnlineContext context1;
            private readonly IMapper mapper1;

            public Manejador(CursosOnlineContext context, IMapper mapper){
                context1 = context;
                mapper1 = mapper;
            }      
            
            public async Task<List<CursoDto>> Handle(ListaCursos request, CancellationToken cancellationToken)
            {
                var cursos = await context1.Curso
                .Include(x => x.ComentarioLista)
                .Include(x => x.PrecioPromocion)
                .Include(x => x.InstructorLink)
                .ThenInclude(x => x.Instructor).ToListAsync();

                var cursosDto = mapper1.Map<List<Curso>, List<CursoDto>>(cursos); //Mapea desde la <clase origen , hasta la resultante>

                return cursosDto;
            }
        }
    }
}