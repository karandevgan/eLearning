using System;
using System.Linq;
using Learning.Data.Entities;

namespace Learning.Data {
    public class LearningRepository : ILearningRepository {
        private LearningContext context;

        public LearningRepository(LearningContext context) {
            this.context = context;
        }

        public bool CourseExists(int courseId) {
            return context.Courses.Any(c => c.Id == courseId);
        }

        public bool DeleteCourse(int id) {
            try {
                var entity = context.Courses.Find(id);
                if (entity != null) {
                    context.Courses.Remove(entity);
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }
        }

        public bool DeleteStudent(int studentId) {
            try {
                var entity = context.Students.Find(studentId);
                if (entity != null) {
                    context.Students.Remove(entity);
                    return true;
                }
                return false;
            }
            catch {
                return false;
            }
        }

        public int EnrollStudentInCourse(int studentId, int courseId, Enrollment enrollment) {
            try {
                if (context.Enrollments.Any(e => e.Course.Id == courseId && e.Student.Id == studentId))
                    return 2;

                context.Database.ExecuteSqlCommand("INSERT INTO ENROLLMENTS VALUES (@p0, @p1, @p2)", 
                    enrollment.EnrollmentDate, courseId.ToString(), studentId.ToString());
                return 1;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbex) {
                foreach (var eve in dbex.EntityValidationErrors) {
                    string line = String.Format("Entity of type '{0}' in state '{1}' has the following validation errors: ", 
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);

                    foreach (var ve in eve.ValidationErrors) {
                        line = string.Format("- Property: '{0}', Error: '{1}'", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return 0;
            }
            catch (Exception ex) {
                return 0;
            }
        }

        public IQueryable<Course> GetAllCourses() {
            return context.Courses
                .Include("CourseSubject")
                .Include("CourseTutor")
                .AsQueryable();
        }

        public IQueryable<Student> GetAllStudentsSummary() {
            throw new NotImplementedException();
        }

        public IQueryable<Student> GetAllStudentsWithEnrollments() {
            return context.Students
                .Include("Enrollments")
                .Include("Enrollments.Course")
                .Include("Enrollments.Course.CourseSubject")
                .Include("Enrollments.Course.CourseTutor")
                .AsQueryable();
        }

        public IQueryable<Subject> GetAllSubjects() {
            return context.Subjects.AsQueryable();
        }

        public Course GetCourse(int courseId, bool includeEnrollments = true) {
            var courses = context.Courses
                .Include("CourseSubject")
                .Include("CourseTutor");

            if (includeEnrollments) {
                return courses
                    .Include("Enrollments")
                    .Where(c => c.Id == courseId)
                    .SingleOrDefault();
            }
            else {
                return courses
                    .Where(c => c.Id == courseId)
                    .SingleOrDefault();
            }
        }

        public IQueryable<Course> GetCoursesBySubject(int subjectId) {
            return context.Courses
                .Include("CourseSubject")
                .Include("CourseTutor")
                .Where(c => c.CourseSubject.Id == subjectId)
                .AsQueryable();
        }

        public IQueryable<Student> GetEnrolledStudentsInCourse(int courseId) {
            return context.Students
                .Include("Enrollments")
                .Where(s => s.Enrollments.Any(e => e.Course.Id == courseId))
                .AsQueryable();
        }

        public Student GetStudent(string userName) {
            return context.Students
                .Include("Enrollments")
                .Where(s => s.UserName == userName)
                .SingleOrDefault();
        }

        public Student GetStudentEnrollments(string userName) {
            throw new NotImplementedException();
        }

        public Subject GetSubject(int subjectId) {
            return context.Subjects.Find(subjectId);
        }

        public Tutor GetTutor(int tutorId) {
            return context.Tutors
                .Find(tutorId);
        }

        public bool Insert(Course course) {
            try {
                context.Courses.Add(course);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool Insert(Student student) {
            try {
                context.Students.Add(student);
                return true;
            }
            catch {
                return false;
            }
        }

        public bool LoginStudent(string userName, string password) {
            return context.Students.Any(s => s.UserName == userName && s.Password == password);
        }

        public bool SaveAll() {
            return context.SaveChanges() > 0;
        }

        public bool Update(Course originalCourse, Course updatedCourse) {
            context.Entry(originalCourse).CurrentValues.SetValues(updatedCourse);
            return true;
        }

        public bool Update(Student originalStudent, Student updatedStudent) {
            context.Entry(originalStudent).CurrentValues.SetValues(updatedStudent);
            return true;
        }
    }
}
