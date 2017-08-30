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
    //Note on the DataAccess class here we write the connection and the sql function like update delete  
    //and in the controller we call it .

    public class EmployeeDataAccess : IRepository<EmployeeListResult>
    {
        public object Employee { get; private set; }

        //------------------
        //to get all the Employees, Note that <EmployeeListResult> is the output list of employee.
        public List<EmployeeListResult> GetAll()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var result = connection.Query<EmployeeListResult>("select * from Employee");
                //return the list of employee
                return result.ToList();
            }
        }
//-----------------------------------------------------------------------------------------------

            //get employee by Id
        public EmployeeListResult Get(int EmpId)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                connection.Open();

                var result = connection.QueryFirstOrDefault<EmployeeListResult>("Select * From Employee where EmployeeId = @Id", new { Id = EmpId });
                //return the employee
                return result;
            }
        }
//-----------------------------------------------------------------------------------------------
        //delete employee by Id
        public void Delete(int EmpId)
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                Connection.Open();
                var Result = Connection.Execute("Delete From Employee where EmployeeId = @Id", new { Id = EmpId});
                //note there is no return as it is void 
            }
        }

//-----------------------------------------------------------------------------------------------
        //update the lastName by passing the employee object(the employee id and the last name)
        public void Update(EmployeeListResult employee)
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                Connection.Open();
                var result = Connection.Execute("update Employee set LastName = @LastName where EmployeeId = @Id",
                                                 new { Id = employee.EmployeeId, LastName = employee.LastName});
                //note there is no return as it is void
            }
        }

//-----------------------------------------------------------------------------------------------


    }
}