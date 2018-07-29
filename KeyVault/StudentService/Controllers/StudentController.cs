using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StudentDataService.Controllers
{
    public class StudentController : ApiController
    {
        StudentDataManager manager;
        public StudentController()
        {
            manager = new StudentDataManager();
            manager.Initialize();
        }

        public IList<StudentEntry> GetStudent()
        {
            return manager.GetStudents();
        }
        
        public HttpResponseMessage PostStudent([FromBody]StudentEntry student)
        {
            var statusCode = HttpStatusCode.BadRequest;
            manager.AddStudent(student).ContinueWith(t =>
            {
                statusCode = (HttpStatusCode)t.Result.HttpStatusCode;
            });
            return Request.CreateResponse(statusCode);
        }

        public HttpResponseMessage DeleteStudent([FromUri]string rowKey)
        {
            var statusCode = HttpStatusCode.BadRequest;
            var student = manager.GetStudent(rowKey);
            if(student!=null)
            {
                manager.DeleteStudent(student).ContinueWith(t =>
                {
                    statusCode = (HttpStatusCode)t.Result.HttpStatusCode;
                });
            }
            return Request.CreateResponse(statusCode);
        }
    }
}
