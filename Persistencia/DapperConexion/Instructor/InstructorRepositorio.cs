using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace Persistencia.DapperConexion.Instructor
{
    public class InstructorRepositorio : IInstructor
    {
        private readonly IFactoryConnection factoryConnection1;
        public InstructorRepositorio(IFactoryConnection factoryConnection)
        {
            factoryConnection1 = factoryConnection;
        }
        public async Task<int> Actualizar(Guid instructorId, string nombre, string apellidos, string titulo)
        {
            var storeProcedure = "usp_instructor_editar";

            try{
                var connection = factoryConnection1.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure, new
                {
                    InstructorId = instructorId,
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Titulo = titulo
                },
                    commandType: CommandType.StoredProcedure
                    );
                factoryConnection1.CloseConnection();
                return resultado;
            }catch(Exception e){
                throw new Exception("No se pudo guardar el nuevo instructor", e);   
            }finally{
                factoryConnection1.CloseConnection();
            }
        }

        public async Task<int> Eliminar(Guid id)
        {
            var storeProcedure = "usp_instructor_eliminar";
            try
            {
                var connection = factoryConnection1.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure, new{
                    InstructorId = id
                },
                commandType: CommandType.StoredProcedure
                );
                factoryConnection1.CloseConnection();
                return resultado;
            }
            catch(Exception e){
                throw new Exception("No se pudo guardar el nuevo instructor", e);  
            }
        }

        public async Task<int> Nuevo(string nombre, string apellidos, string titulo)
        {
            var storeProcedure = "usp_instructor_nuevo";

            try
            {
                var connection = factoryConnection1.GetConnection();
                var resultado = await connection.ExecuteAsync(storeProcedure, new
                {
                    InstructorId = Guid.NewGuid(),
                    Nombre = nombre,
                    Apellidos = apellidos,
                    Titulo = titulo
                },
                    commandType: CommandType.StoredProcedure
                    );
                factoryConnection1.CloseConnection();
                return resultado;
            }
            catch (Exception e)
            {

                throw new Exception("No se pudo guardar el nuevo instructor", e);
            }
            finally
            {
                factoryConnection1.CloseConnection();
            }
        }

        public async Task<IEnumerable<InstructorModel>> ObtenerLista()
        {
            IEnumerable<InstructorModel> instructorList = null;
            var storeProcedure = "usp_OBtener_Instructores";
            try
            {
                var connection = factoryConnection1.GetConnection();
                instructorList = await connection.QueryAsync<InstructorModel>(storeProcedure, null, commandType: CommandType.StoredProcedure);
            }
            catch (Exception e)
            {
                throw new Exception("Error en la consulta de datos", e);
            }
            finally
            {
                factoryConnection1.CloseConnection();
            }
            return instructorList;
        }

        public async Task<InstructorModel> ObtenerPorId(Guid id)
        {
            var storeProcedure = "usp_Obtener_Instructores_por_id";
            InstructorModel instructor = null;
            try
            {
                var connection = factoryConnection1.GetConnection();
                instructor = await connection.QueryFirstAsync<InstructorModel>(storeProcedure, 
                new {
                    Id = id
                }, 
                commandType: CommandType.StoredProcedure
                );
                return instructor;
            }
            catch (Exception e)
            {
                
                throw new Exception("No se pudo encontrar el instructor", e);
            }
        }
    }
}