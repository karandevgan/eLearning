﻿using Learning.Data;
using Learning.Data.Entities;
using Learning.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Learning.Web.Controllers {
    public class CoursesController : BaseApiController {
        public CoursesController(ILearningRepository repo) : base(repo) {

        }

        public IEnumerable<CourseModel> Get(int page = 0, int pageSize = 10) {
            IQueryable<Course> query;
            query = TheRepository.GetAllCourses().OrderBy(c => c.CourseSubject.Id);

            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var urlHelper = new UrlHelper(Request);
            var prevLink = page > 0 ? urlHelper.Link("Courses", new { page = page - 1 }) : "";
            var nextLink = page < totalPages - 1 ? urlHelper.Link("Courses", new { page = page + 1 }) : "";

            var paginationHeaders = new {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PrevPageLink = prevLink,
                NextPageLink = nextLink
            };

            System.Web.HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationHeaders));
 
            var results = query
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToList()
                .Select(s => TheModelFactory.Create(s));

            return results;
        }

        public HttpResponseMessage GetCourse(int id) {
            try {
                var course = TheRepository.GetCourse(id);
                if (course != null) {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(course));
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Post([FromBody] CourseModel courseModel) {
            try {
                var entity = TheModelFactory.Parse(courseModel);
                if (entity == null) Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read data properly");

                if (TheRepository.Insert(entity) && TheRepository.SaveAll()) {
                    return Request.CreateResponse(HttpStatusCode.Created, TheModelFactory.Create(entity));
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Could not save to the database");
                }
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPatch]
        [HttpPut]
        public HttpResponseMessage Put(int id, [FromBody] CourseModel courseModel) {
            try {
                var updatedCourse = TheModelFactory.Parse(courseModel);
                if (updatedCourse == null) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Could not read subject/tutor from body");

                var originalCourse = TheRepository.GetCourse(id, false);
                if (originalCourse == null || originalCourse.Id != id) {
                    return Request.CreateErrorResponse(HttpStatusCode.NotModified, "Course is not found");
                }
                else {
                    updatedCourse.Id = id;
                }

                if (TheRepository.Update(originalCourse, updatedCourse) && TheRepository.SaveAll()) {
                    return Request.CreateResponse(HttpStatusCode.OK, TheModelFactory.Create(updatedCourse));
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.NotModified);
                }
            }
            catch (Exception ex) {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id) {
            try {
                var course = TheRepository.GetCourse(id);
                if (course == null) return Request.CreateResponse(HttpStatusCode.NotFound);

                if (course.Enrollments.Count > 0) {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Cannot delete course with students enrolled in it.");
                }

                if (TheRepository.DeleteCourse(id) && TheRepository.SaveAll()) {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex) {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
