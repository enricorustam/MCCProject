using ASP.NetCoreProject.Repository.Interface;
using ASP.NetCoreProject.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreProject.Repository
{
    public class SupervisorRepository : ISupervisorRepository
    {
        IConfiguration _configuration;
        DynamicParameters parameters = new DynamicParameters();
        public SupervisorRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int Create(SupervisorVM supervisor)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_InsertSupervisor";
                parameters.Add("Name", supervisor.Name);
                var InsertSupervisor = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return InsertSupervisor;
            }
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_DeleteSupervisor";
                parameters.Add("Id", Id);
                var DeleteSupervisor = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return DeleteSupervisor;

            }
        }

        public IEnumerable<SupervisorVM> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetAllSupervisor";
                var getAllSupervisor = connection.Query<SupervisorVM>(procName, commandType: CommandType.StoredProcedure);
                return getAllSupervisor;
            }
        }

        public async Task<IEnumerable<SupervisorVM>> GetById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetIdSupervisor";
                parameters.Add("Id", Id);
                var getIdSupervisor = await connection.QueryAsync<SupervisorVM>(procName, parameters, commandType: CommandType.StoredProcedure);

                return getIdSupervisor;
            }
        }

        public int Update(SupervisorVM supervisor, int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_EditSupervisor";
                parameters.Add("Id", Id);
                parameters.Add("Name", supervisor.Name);
                var EditSupervisor = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return EditSupervisor;
            }
        }
    }
}
