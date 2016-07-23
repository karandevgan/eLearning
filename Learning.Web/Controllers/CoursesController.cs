using Learning.Data;
using Learning.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Learning.Web.Controllers {
    public class CoursesController : ApiController
    {
        ILearningRepository repository;

        public CoursesController() : base() {
            this.repository = new LearningRepository(new LearningContext());
        }

        public List<Course> Get() {
            return repository.GetAllCourses().ToList();
        }

        public Course GetCourse(int id) {
            return repository.GetCourse(id);
        }
    }
}
