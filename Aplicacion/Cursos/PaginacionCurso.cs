using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Paginacion;

namespace Aplicacion.Cursos
{
    public class PaginacionCurso
    {
        public class Ejecuta : IRequest<PaginacionModel>{
            public string Titulo { get; set; }
            public int numeroPagina { get; set; }
            public int CantidadElementos { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta, PaginacionModel>
        {   
            private readonly IPaginacion paginacion1;
            public Manejador(IPaginacion paginacion){
                paginacion1 = paginacion;
            }
            public async Task<PaginacionModel> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var storeProcedure = "usp_obtener_curso_paginacion";
                var ordenamiento = "Titulo"; 
                var parametros = new Dictionary<string, object>();
                parametros.Add("NombreCurso", request.Titulo);
                return await paginacion1.devolverPaginacion(storeProcedure, request.numeroPagina, request.CantidadElementos, parametros, ordenamiento);
            }
        }
    }
}