using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Persistencia.DapperConexion.Instructor;

namespace Aplicacion.Instructores
{
    public class Eliminar
    {
        public class Ejecuta : IRequest
        {
            public Guid InstructorId { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly IInstructor instructorRepository1;
            public Manejador(IInstructor instructorRepository)
            {
                instructorRepository1 = instructorRepository;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var resultado = await instructorRepository1.Eliminar(request.InstructorId);
                if (resultado > 0)
                {
                    return Unit.Value;
                }
                throw new Exception("No se pudo editar al instructor");
            }
        }
    }
}