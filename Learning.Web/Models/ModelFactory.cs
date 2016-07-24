﻿using Learning.Data.Entities;
using System.Net.Http;

namespace Learning.Web.Models {
    public class ModelFactory {

        private System.Web.Http.Routing.UrlHelper _urlHelper;

        public ModelFactory(HttpRequestMessage request) {
            _urlHelper = new System.Web.Http.Routing.UrlHelper(request);
        }

        public CourseModel Create(Course course) {
            return new CourseModel() {
                Url = _urlHelper.Link("Courses", new { id = course.Id }),
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Duration = course.Duration,
                Subject = Create(course.CourseSubject),
                Tutor = Create(course.CourseTutor)
            };
        }

        public TutorModel Create(Tutor tutor) {
            return new TutorModel() {
                Id = tutor.Id,
                Email = tutor.Email,
                UserName = tutor.UserName,
                FirstName = tutor.FirstName,
                LastName = tutor.LastName,
                Gender = tutor.Gender
            };
        }

        public SubjectModel Create(Subject subject) {
            return new SubjectModel() {
                Id = subject.Id,
                Name = subject.Name
            };
        }

        public EnrollmentModel Create(Enrollment enrollment) {
            return new EnrollmentModel() {
                EnrollmentDate = enrollment.EnrollmentDate,
                Course = Create(enrollment.Course)
            };
        }
    }
}