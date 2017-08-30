using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Dapper;
using OurFirstApi.Models;
using OurFirstApi.DataAccess;

namespace OurFirstApi.Controllers
{
    //in the controller we use the functions from dataAccessso we don't need to open the connection
    [RoutePrefix("api/employee")]
    //api/employee
    public class EmployeesController : ApiController
    {
//-----------------------------------------------------------------------------------------------------------------------
        //select all the employee by using the function GetAll() 
        //api/employee
        [HttpGet, Route("")]
        public HttpResponseMessage Get()
        {
            // using (var connection =
            //new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            //{
            try
            {
                // connection.Open();

                //var result = connection.Query<EmployeeListResult>("select * " +
                //  "from Employee");
                //return Request.CreateResponse(HttpStatusCode.OK, result);

                var employeeData = new EmployeeDataAccess();
                //employeeData is an instance of the EmployeeDataAccess class so it can use it's functions
                var AllemployeeList = employeeData.GetAll();
                return Request.CreateResponse(HttpStatusCode.OK, AllemployeeList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Query blew up");
            }
            //}
        }

        //-----------------------------------------------------------------------------------------------------------------------
        // select employee with specific id
        //api/employees/3000
        [HttpGet, Route("{id}")]
        public HttpResponseMessage Get(int id)
        {
            // using (var connection =
            //new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            // {
            try
            {
                // connection.Open()
                // var result =
                // connection.Query<EmployeeListResult>("Select * From Employee where EmployeeId = @id",
                //new {id = id}).FirstOrDefault();
                var repo = new EmployeeDataAccess();
                //repo is an instance of the EmployeeDataAccess class so it can use it's functions
                var result = repo.Get(id);

                if (result == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with the Id {id} was not found");
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            // }
        }

        //------------------------------------------------------------------------------------------------------
        //Select Employee by first name
        [HttpGet, Route("name/{firstName}")]

        public HttpResponseMessage Get(string firstName)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    connection.Open();

                    var result =
                        connection.Query<EmployeeListResult>("Select * From Employee where FirstName = @firstname",
                            new { firstName }).FirstOrDefault();

                    if (result == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Employee with the Name {firstName} was not found");
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------
        //adding new employee (passing the whole object)
        [HttpPost,Route("")]
        public HttpResponseMessage Post(EmployeeListResult Employee)
        {
            using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            {
                try
                {
                    Connection.Open();
                    var result = Connection.Execute("Insert into Employee(FirstName, LastName) Values(@FirstName, @LastName)",
                                                      new { FirstName = Employee.FirstName, LastName = Employee.LastName });
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                catch (Exception Ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex);
                }
            }
        }

//-----------------------------------------------------------------------------------------------------------------------
        //Updating LastName ( passing Id and lastname)
        [HttpPut,Route("")]
        public HttpResponseMessage Put( EmployeeListResult Employee)
        {
            // using (var Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            //{
            var Updating = new EmployeeDataAccess();
                try
                {
                // Connection.Open();
                // var result = Connection.Execute("update Employee set LastName = @LastName where EmployeeId = @Id",
                // new { LastName = Employee.LastName, Id = Employee.EmployeeId });
                //return Request.CreateResponse(HttpStatusCode.OK, result);
                
                    Updating.Update(Employee);
                return Request.CreateResponse(HttpStatusCode.OK);


            }
            catch (Exception Ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex);
                }
            }

        //-----------------------------------------------------------------------------------------------------------------------
        //delete employee with specific id 
        [HttpDelete, Route("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var DataAccess = new EmployeeDataAccess();
            //new instance
            try
            {
                DataAccess.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception Ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex);
            }

            //using (var Connection= new SqlConnection(ConfigurationManager.ConnectionStrings["Chinook"].ConnectionString))
            //{
            //    try
            //    {
            //        Connection.Open();
            //        var Result = Connection.Execute("Delete From Employee where EmployeeId = @EmpId", new {EmpId = Id });

            //        return Request.CreateResponse(HttpStatusCode.OK,Result);

            //    }
            //    catch(Exception Ex)
            //    {
            //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Ex);
            //    }
            //}
        }
    }

        
}
