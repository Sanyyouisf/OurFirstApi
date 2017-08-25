using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using OurFirstApi.Models;

namespace OurFirstApi.Controllers
{
    //api/employees
    public class EmployeesController : ApiController
    {
        //api/employees
        public HttpResponseMessage Get()
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    var result = connection.Query<EmployeeListResult>("select * " +
                                                                      "from Employee");
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query blew up");
                }
            }
        }

        //api/employees/3000
        public HttpResponseMessage Get(int id)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    var result =
                        connection.Query<EmployeeListResult>("Select * From Employee where EmployeeId = @id",
                            new {id = id}).FirstOrDefault();

                    if (result == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound,$"Employee with the Id {id} was not found");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }

        //adding new employee (passing the whole object)
        public HttpResponseMessage Post (EmployeeListResult Employee)
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    Connection.Open();
                    var result = Connection.Execute("Insert into Employee(FirstName, LastName) Values(@FirstName, @LastName)", 
                                                      new {FirstName =Employee.FirstName, LastName = Employee.LastName });
                    return Request.CreateResponse(HttpStatusCode.OK,result);
                }
                catch(Exception Ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex);
                }
            }
        }



        //Updating LastName ( passing Id and lastname)
        public HttpResponseMessage Put( EmployeeListResult Employee)
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    Connection.Open();
                    var result = Connection.Execute("update Employee set LastName = @LastName where EmployeeId = @Id",
                                                     new { LastName=Employee.LastName , Id=Employee.EmployeeId});
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                 catch (Exception Ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex);
                }

            }
        }
    }
}
