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
    public class AdminRepository : IAdminRepositoy
    {
        IConfiguration _configuration;
        DynamicParameters parameters = new DynamicParameters();
        public AdminRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int Create(AdminVM admin)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_InsertAdmin";
                parameters.Add("Username", admin.Username);
                parameters.Add("Password", admin.Password);
                var InsertAdmin = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return InsertAdmin;
            }


        }
        public int Delete(int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_DeleteAdmin";
                parameters.Add("Id", Id);
                var DeleteAdmin = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return DeleteAdmin;

            }
        }

        public IEnumerable<AdminVM> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_GetAllAdmin";
                var getAllAdmin = connection.Query<AdminVM>(procName, commandType: CommandType.StoredProcedure);
                return getAllAdmin;
            }
        }

        public Task<IEnumerable<AdminVM>> GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public int Update(AdminVM admin, int Id)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("myConn")))
            {
                var procName = "SP_EditAdmin";
                parameters.Add("Id", Id);
                parameters.Add("Username", admin.Username);
                parameters.Add("Password", admin.Password);
                var EditAdmin = connection.Execute(procName, parameters, commandType: CommandType.StoredProcedure);
                return EditAdmin;
            }
        }
    }
}