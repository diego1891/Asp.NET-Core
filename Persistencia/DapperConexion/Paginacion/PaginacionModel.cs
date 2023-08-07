using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion.Paginacion
{
    //Armo la clase de lo que retornará el sqlserver
    public class PaginacionModel
    {
        public List<IDictionary<string, object>> ListaRecords { get; set;}
        public int TotalRecords { get; set;}
        public int NumeroPaginas { get; set;}
    }
}