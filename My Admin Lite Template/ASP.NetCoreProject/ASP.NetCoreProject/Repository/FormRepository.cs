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
    public class FormRepository : IFormRepository
    {
        IConfiguration _configuration;
        DynamicParameters parameters = new DynamicParameters();

        public FormRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int Create(FormVM form)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_InsertForm";
                parameters.Add("employee", form.employeeId);
                parameters.Add("StartDate", form.StartDate);
                parameters.Add("EndDate", form.EndDate);
                parameters.Add("Duration", form.Duration);
                parameters.Add("supervisor", form.supervisorId);
                parameters.Add("department", form.departmentId);
                var InsertForm = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return InsertForm;
            }
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_DeleteForm";
                parameters.Add("Id", Id);
                var DeleteForm = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return DeleteForm;
            }
        }

        public async Task<IEnumerable<FormVM>> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetAllForm";
                var getAllForm = await connection.QueryAsync<FormVM>(procName, commandType: CommandType.StoredProcedure);
                return getAllForm;
            }
        }

        public FormVM GetById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetIdForm";
                parameters.Add("Id", Id);
                var getIdForm = connection.Query<FormVM>(procName, parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                return getIdForm;
            };
        }

        public int Update(FormVM form, int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_EditForm";
                parameters.Add("Id", Id);
                parameters.Add("employee", form.employeeId);
                parameters.Add("StartDate", form.StartDate);
                parameters.Add("EndDate", form.EndDate);
                parameters.Add("Duration", form.Duration);
                parameters.Add("supervisor", form.supervisorId);
                parameters.Add("department", form.departmentId);
                var EditForm = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return EditForm;
            }
        }
    }
}
