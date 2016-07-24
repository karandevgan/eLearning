namespace Learning.Data.Debug {
    class Program {
        static void Main(string[] args) {
            ILearningRepository repo = new LearningRepository(new LearningContext());
            repo.GetAllCourses();  
        }
    }
}
