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
    public class DepartmentRepository : IDepartmentRepository
    {
        IConfiguration _configuration;
        DynamicParameters parameters = new DynamicParameters();
        public DepartmentRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int Create(DepartmentVM department)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_InsertDepartment";
                parameters.Add("Name", department.Name);
                var InsertDepartment = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return InsertDepartment;
            }
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_DeleteDepartment";
                parameters.Add("Id", Id);
                var DeleteDepartment = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return DeleteDepartment;

            }
        }

        public async Task<IEnumerable<DepartmentVM>> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetAllDepartment";
                var getAllDepartment = await connection.QueryAsync<DepartmentVM>(procName, commandType: CommandType.StoredProcedure);
                return getAllDepartment;
            }
        }

        public DepartmentVM GetById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetIdDepartment";
                parameters.Add("Id", Id);
                var getIdDepartment = connection.Query<DepartmentVM>(procName, parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                return getIdDepartment;
            }
        }

        public int Update(DepartmentVM department, int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_EditDepartment";
                parameters.Add("Id", Id);
                parameters.Add("Name", department.Name);
                var EditDepartment = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return EditDepartment;
            }
        }
    }
}
