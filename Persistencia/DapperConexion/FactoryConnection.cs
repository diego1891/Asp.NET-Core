using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Persistencia.DapperConexion
{
    public class FactoryConnection : IFactoryConnection
    {
        private IDbConnection connection1;
        private readonly IOptions<ConexionConfiguracion> configs1;
        public FactoryConnection(IDbConnection connection){
            connection1 = connection;
        }

        public void CloseConnection()
        {
            if(connection1 != null && connection1.State == ConnectionState.Open){
                connection1.Close();
            }
        }

        public IDbConnection GetConnection()
        {
            /*Se evalua si la cadena de conexion existe, si no la crea*/
            if(connection1 == null){
                connection1 = new SqlConnection(configs1.Value.ConexionSQL);
            }
            /*Se evalua si está abierta y si no lo está la abre*/
            if(connection1.State != ConnectionState.Open){
                connection1.Open();
            }
            return connection1;
        }
    }
}