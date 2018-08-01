using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using ApiService.Models;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;

namespace ApiService.Controllers
{
    /// <summary>
    /// Student Controller
    /// </summary>
    public class StudentController : ApiController
    {
        /// <summary>
        /// Gets All Students
        /// </summary>
        /// <returns></returns>
        [SwaggerOperation("GetAll")]
        public IEnumerable<STUDENT> GetStudent()
        {
            var context = new StudentDataEntities();
            context.STUDENTs.Load();
            return context.STUDENTs.ToArray();
        }
    }
}
