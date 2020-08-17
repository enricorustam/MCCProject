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
    public class ValidationRepository : IValidationRepository
    {
        IConfiguration _configuration;
        DynamicParameters parameters = new DynamicParameters();
        public ValidationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int Create(ValidationVM validation)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_InsertValidation";
                parameters.Add("Action", validation.Action);
                parameters.Add("supervisor", validation.supervisorId);
                parameters.Add("form", validation.formId);
                var insertValidation = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return insertValidation;
            }
        }

        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_DeleteValidation";
                parameters.Add("Id", Id);
                var DeleteValidation = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return DeleteValidation;
            }
        }

        public async Task<IEnumerable<ValidationVM>> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetAllValidation";
                var getAllValidation = await connection.QueryAsync<ValidationVM>(procName, commandType: CommandType.StoredProcedure);
                return getAllValidation;
            }
        }

        public ValidationVM GetById(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetIdValidation";
                parameters.Add("Id", Id);
                var getIdValidation = connection.Query<ValidationVM>(procName, parameters, commandType: CommandType.StoredProcedure).SingleOrDefault();
                return getIdValidation;
            };
        }

        public async Task<IEnumerable<ValidationVM>> getValidationChart()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetValidationChart";
                var getAll = await connection.QueryAsync<ValidationVM>(procName, commandType: CommandType.StoredProcedure);
                return getAll;
            }
        }

        public int Update(ValidationVM validation, int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_EditValidation";
                parameters.Add("Id", Id);
                parameters.Add("Action", validation.Action);
                parameters.Add("supervisor", validation.supervisorId);
                parameters.Add("form", validation.formId);
                var EditValidation = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return EditValidation;
            }
        }
    }
}
