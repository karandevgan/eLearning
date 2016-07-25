using Learning.Data;
using Learning.Data.Entities;
using Learning.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Learning.Web.Controllers
{
    public class StudentsController : BaseApiController
    {
        public StudentsController(ILearningRepository repo) : base(repo)
        {

        }

        public IEnumerable<StudentBaseModel> Get(bool summary = true)
        {
            IQueryable<Student> query;
            query = TheRepository.GetAllStudentsWithEnrollments().OrderBy(s => s.LastName);
            IEnumerable<StudentBaseModel> result;
            if (summary)
            {
                result = query
                    .ToList()
                    .Select(s => TheModelFactory.CreateSummary(s));
            }
            else
            {
                result = query
                    .ToList()
                    .Select(s => TheModelFactory.Create(s));
            }
            return result;
        }

        public HttpResponseMessage GetStudent(string userName)
        {
            try
            {
                var student = TheRepository.GetStudent(userName);
                if (student == null) return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect username of the student");

                return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(student));

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Post([FromBody] Student student)
        {
            try
            {
                TheRepository.Insert(student);
                if (TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.CreateSummary(student));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not save to the database.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        [HttpPatch]
        public HttpResponseMessage Put(string userName, [FromBody] Student student)
        {
            try
            {
                var originalStudent = TheRepository.GetStudent(userName);
                if (originalStudent == null || originalStudent.UserName != userName)
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Student is not found");
                }
                else
                {
                    student.Id = originalStudent.Id;
                }

                if (TheRepository.Update(originalStudent, student) && TheRepository.SaveAll())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(student));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotModified, "Cannot save to the database.");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public HttpResponseMessage Delete(string userName)
        {
            try
            {
                var student = TheRepository.GetStudent(userName);
                if (student == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Incorrect username.");
                }
                else
                {
                    if (TheRepository.DeleteStudent(student.Id) && TheRepository.SaveAll())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Student with username " + userName + " deleted.");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotModified, "Error writing to the database.");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
