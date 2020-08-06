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
    public class EmployeeRepository : IEmployeeRepository
    {
        IConfiguration _configuration;
        DynamicParameters parameters = new DynamicParameters();
        public EmployeeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int Create(EmployeeVM employee)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_InsertEmployee";
                parameters.Add("NIP", employee.NIP);
                parameters.Add("Name", employee.Name);
                var InsertEmployee = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return InsertEmployee;
            }
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_DeleteEmployee";
                parameters.Add("Id", Id);
                var DeleteEmployee = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return DeleteEmployee;

            }
        }

        public IEnumerable<EmployeeVM> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetAllEmployee";
                var getAllEmployee = connection.Query<EmployeeVM>(procName, commandType: CommandType.StoredProcedure);
                return getAllEmployee;
            }
        }

        public async Task<IEnumerable<EmployeeVM>> GetById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetIdEmployee";
                parameters.Add("Id", Id);
                var getIdEmployee = await connection.QueryAsync<EmployeeVM>(procName, parameters, commandType: CommandType.StoredProcedure);

                return getIdEmployee;
            }
        }

        public int Update(EmployeeVM employee, int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_EditEmployee";
                parameters.Add("Id", Id);
                parameters.Add("NIP", employee.NIP);
                parameters.Add("Name", employee.Name);
                var EditEmployee = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return EditEmployee;
            }
        }
    }
}
