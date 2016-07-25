using Learning.Data.Entities;
using System.Net.Http;
using System;
using Learning.Data;
using System.Linq;

namespace Learning.Web.Models
{
    public class ModelFactory
    {

        private System.Web.Http.Routing.UrlHelper _urlHelper;
        private ILearningRepository _repo;

        public ModelFactory(HttpRequestMessage request, ILearningRepository repo)
        {
            _urlHelper = new System.Web.Http.Routing.UrlHelper(request);
            _repo = repo;
        }

        public StudentModel Create(Student student)
        {
            return new StudentModel()
            {
                Url = _urlHelper.Link("Students", new { userName = student.UserName }),
                Id = student.Id,
                UserName = student.UserName,
                FirstName = student.FirstName,
                LastName = student.LastName,
                DateOfBirth = student.DateOfBirth,
                Email = student.Email,
                Gender = student.Gender,
                RegistrationDate = student.RegistrationDate,
                EnrollmentsCount = student.Enrollments.Count,
                Enrollments = student.Enrollments.Select(e => Create(e))
            };
        }

        public StudentBaseModel CreateSummary(Student student)
        {
            return new StudentBaseModel()
            {
                Url = _urlHelper.Link("Students", new { userName = student.UserName }),
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Gender = student.Gender,
                EnrollmentsCount = student.Enrollments.Count                
            };
        }

        public CourseModel Create(Course course)
        {
            return new CourseModel()
            {
                Url = _urlHelper.Link("Courses", new { id = course.Id }),
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Duration = course.Duration,
                Subject = Create(course.CourseSubject),
                Tutor = Create(course.CourseTutor)
            };
        }

        public TutorModel Create(Tutor tutor)
        {
            return new TutorModel()
            {
                Id = tutor.Id,
                Email = tutor.Email,
                UserName = tutor.UserName,
                FirstName = tutor.FirstName,
                LastName = tutor.LastName,
                Gender = tutor.Gender
            };
        }

        public SubjectModel Create(Subject subject)
        {
            return new SubjectModel()
            {
                Id = subject.Id,
                Name = subject.Name
            };
        }

        public EnrollmentModel Create(Enrollment enrollment)
        {
            return new EnrollmentModel()
            {
                EnrollmentDate = enrollment.EnrollmentDate,
                Course = Create(enrollment.Course)
            };
        }

        public Course Parse(CourseModel model)
        {
            try
            {
                var course = new Course()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Duration = model.Duration,
                    CourseSubject = _repo.GetSubject(model.Subject.Id),
                    CourseTutor = _repo.GetTutor(model.Tutor.Id)
                };
                return course;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}