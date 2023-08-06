using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Cursos;
using AutoMapper;
using Dominio;

namespace Aplicacion
{
    public class MappingProfile : Profile
    {
        public MappingProfile(){
            CreateMap<Curso, CursoDto>()
            /*Los for member son para indicar que el atributo x de curso se llenará desde el atributo y de tal*/
            .ForMember(x => x.Instructores, y => y.MapFrom(z => z.InstructorLink.Select(a => a.Instructor).ToList()))
            .ForMember(x => x.Comentarios, y=> y.MapFrom(z => z.ComentarioLista))
            .ForMember(x => x.Precio, y => y.MapFrom(y => y.PrecioPromocion)); 
            CreateMap<CursoInstructor, CursoInstructorDto>();
            CreateMap<Instructor, InstructorDto>();
            CreateMap<Comentario, ComentarioDto>();
            CreateMap<Precio, PrecioDto>();
        }
    }
}