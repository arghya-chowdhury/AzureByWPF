using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using Service.Models;

namespace Service.Controllers
{
    public class StudentController : ApiController
    {
        StudentDataEntities context = new StudentDataEntities();

        public HttpResponseMessage GetStudent()
        {
            context.STUDENTs.Load();

            var studs = context.STUDENTs.ToArray();
            var response = Request.CreateResponse(studs);
            return response;
        }
    }
}
