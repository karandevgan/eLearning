using Learning.Data;
using Learning.Data.Entities;
using Learning.Web.Filters;
using Learning.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Learning.Web.Controllers {
    public class StudentsController : BaseApiController {
        public StudentsController(ILearningRepository repo) : base(repo) {

        }

        public IEnumerable<StudentBaseModel> Get(bool summary = true, int page = 0, int pageSize = 10) {
            IQueryable<Student> query;
            query = TheRepository.GetAllStudentsWithEnrollments().OrderBy(s => s.LastName);

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var urlHelper = new UrlHelper(Request);
            var prevLink = page > 0 ? urlHelper.Link("Students", new { summary = summary, page = page - 1, pageSize = pageSize }) : "";
            var nextLink = page < totalPages - 1 ? urlHelper.Link("Students", new { summary = summary, page = page + 1, pageSize = pageSize }) : "";

            var paginationHeader = new {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageLink = prevLink,
                NextPageLink = nextLink
            };

            System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeader));

            var studentList = query
                    .Skip(pageSize * page)
                    .Take(pageSize)
                    .ToList();

            IEnumerable<StudentBaseModel> result;
            if (summary) {
                result = studentList 
                    .Select(s => TheModelFactory.CreateSummary(s));
            }
            else {
                result = studentList
                    .Select(s => TheModelFactory.Create(s));
            }
            return result;
        }
        [LearningAuthorizeAttribute]
        public HttpResponseMessage GetStudent(string userName) {
            try {
                var student = TheRepository.GetStudent(userName);
                if (student == null) return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect username of the student");

                return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(student));

            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }


        public HttpResponseMessage Post([FromBody] Student student) {
            try {
                TheRepository.Insert(student);
                if (TheRepository.SaveAll()) {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.CreateSummary(student));
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not save to the database.");
                }
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [LearningAuthorizeAttribute]
        [HttpPut]
        [HttpPatch]
        public HttpResponseMessage Put(string userName, [FromBody] Student student) {
            try {
                var originalStudent = TheRepository.GetStudent(userName);
                if (originalStudent == null || originalStudent.UserName != userName) {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Student is not found");
                }
                else {
                    student.Id = originalStudent.Id;
                }

                if (TheRepository.Update(originalStudent, student) && TheRepository.SaveAll()) {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(student));
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Cannot save to the database.");
                }
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public HttpResponseMessage Delete(string userName) {
            try {
                var student = TheRepository.GetStudent(userName);
                if (student == null) {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect username.");
                }
                else {
                    if (TheRepository.DeleteStudent(student.Id) && TheRepository.SaveAll()) {
                        return Request.CreateResponse(HttpStatusCode.OK, "Student with username " + userName + " deleted.");
                    }
                    else {
                        return Request.CreateResponse(HttpStatusCode.NotModified, "Error writing to the database.");
                    }
                }
            }
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
