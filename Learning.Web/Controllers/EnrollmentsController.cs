using Learning.Data;
using Learning.Data.Entities;
using Learning.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Learning.Web.Controllers {
    public class EnrollmentsController : BaseApiController {
        public EnrollmentsController(ILearningRepository repo) : base(repo) {

        }

        public HttpResponseMessage Get(int courseId) {
            if (!TheRepository.CourseExists(courseId)) {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Course not found.");
            }
            IQueryable<Student> query;
            query = TheRepository.GetEnrolledStudentsInCourse(courseId)
                .OrderBy(s => s.LastName);

            var result = query
                .ToList()
                .Select(s => TheModelFactory.CreateSummary(s));

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        public HttpResponseMessage Post(int courseId, [FromUri] string userName, [FromBody] Enrollment enrollment) {
            try {
                if (!TheRepository.CourseExists(courseId)) return Request.CreateResponse(HttpStatusCode.NotFound, "Could not find course.");

                var student = TheRepository.GetStudent(userName);
                if (student == null) return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not find student.");

                var result = TheRepository.EnrollStudentInCourse(student.Id, courseId, enrollment);
                if (result == 1) {
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else if (result == 2) {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Student already enrolled in the course");
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
