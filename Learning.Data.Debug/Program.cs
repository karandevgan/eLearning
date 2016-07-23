using Learning.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learning.Data.Debug {
    class Program {
        static void Main(string[] args) {
            ILearningRepository repo = new LearningRepository(new LearningContext());
            repo.GetAllCourses();  
        }
    }
}
