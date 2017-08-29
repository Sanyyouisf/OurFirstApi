using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using Dapper;
using OurFirstApi.Models;

namespace OurFirstApi.DataAccess
{
    //note on the DataAccess class here we write the sql function like update delete  
    //and in the controller we call it .

    public class EmployeeDataAccess : IRepository<EmployeeListResult>
    {
        public List<EmployeeListResult> GetAll()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var result = connection.Query<EmployeeListResult>("select * " +
                                                                  "from Employee");
                return result.ToList();
            }
        }

//-----------------------------------------------------------------------------------------------
        public EmployeeListResult Get(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var result = connection.QueryFirstOrDefault<EmployeeListResult>(
                    "Select * From Employee where EmployeeId = @id",
                    new { id = id });

                return result;
            }
        }

//-----------------------------------------------------------------------------------------------

        public void Delete(int id)
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                Connection.Open();
                var Result = Connection.Execute("Delete From Employee where EmployeeId = @EmpId", new { EmpId = id });
            }
        }

//-----------------------------------------------------------------------------------------------

        public void Update(EmployeeListResult entityToUpdate)
        {
            throw new NotImplementedException();
        }

//-----------------------------------------------------------------------------------------------


    }
}