using Learning.Data;
using Learning.Data.Entities;
using Learning.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Learning.Web.Controllers {
    public class CoursesController : BaseApiController
    {
        public CoursesController(ILearningRepository repo) : base(repo) {
            
        }

        public IEnumerable<CourseModel> Get() {
            IQueryable<Course> query;
            query = TheRepository.GetAllCourses();

            var results = query
                .ToList()
                .Select(s => TheModelFactory.Create(s));

            return results;
        }

        public HttpResponseMessage GetCourse(int id) {
            try {
                var course = TheRepository.GetCourse(id);
                if (course != null) {
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, TheModelFactory.Create(course));
                }
                else {
                    return Request.CreateResponse(System.Net.HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex) {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
