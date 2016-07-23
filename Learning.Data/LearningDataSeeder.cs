using Learning.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Learning.Data {
    class LearningDataSeeder {
        LearningContext context;

        public LearningDataSeeder(LearningContext context) {
            this.context = context;
        }

        public void Seed() {
            if (context.Courses.Count() > 0) {
                return;
            }

            try {
                foreach (var subjectName in subjects) {
                    var subject = new Subject { Name = subjectName };
                    context.Subjects.Add(subject);
                    context.SaveChanges();
                }

                for (int i = 0; i < tutorNames.Length; i++) {
                    var nameGenderMail = tutorNames[i].Split(',');
                    var tutor = new Tutor {
                        Email = String.Format("{0}_{1}@{2}", nameGenderMail[0], nameGenderMail[1], nameGenderMail[3]),
                        UserName = String.Format("{0}{1}", nameGenderMail[0], nameGenderMail[1]),
                        Password = RandomString(8),
                        FirstName = nameGenderMail[0],
                        LastName = nameGenderMail[1],
                        Gender = ((Enums.Gender)Enum.Parse(typeof(Enums.Gender), nameGenderMail[2]))
                    };
                    context.Tutors.Add(tutor);

                    var courseSubject = context.Subjects.Where(s => s.Id == i + 1).Single();
                    foreach (var CourseDataItem in CoursesSeedData.Where(c=>c.SubjectID == courseSubject.Id)) {
                        var course = new Course {
                            Name = CourseDataItem.CourseName,
                            CourseSubject = courseSubject,
                            CourseTutor = tutor,
                            Duration = new Random().Next(3, 6),
                            Description = String.Format("The course will talk about: {0}", CourseDataItem.CourseName)
                        };
                        context.Courses.Add(course);
                    }
                }
                context.SaveChanges();

                for (int i = 0; i < studentNames.Length; i++) {
                    var nameGenderMail = studentNames[i].Split(',');
                    var student = new Student {
                        Email = String.Format("{0}.{1}@{2}", nameGenderMail[0], nameGenderMail[1], nameGenderMail[3]),
                        UserName = String.Format("{0}{1}", nameGenderMail[0], nameGenderMail[1]),
                        Password = RandomString(8),
                        FirstName = nameGenderMail[0],
                        LastName = nameGenderMail[1],
                        Gender = ((Enums.Gender)Enum.Parse(typeof(Enums.Gender), nameGenderMail[2])),
                        DateOfBirth = DateTime.UtcNow.AddDays(-new Random().Next(7000, 8000)),
                        RegistrationDate = DateTime.UtcNow.AddDays(-new Random().Next(365, 730))
                    };
                    context.Students.Add(student);

                    int maxCourseId = context.Courses.Max(c => c.Id);

                    for (int z = 0; z < 4; z++) {
                        int randomCourseId = new Random().Next(1, maxCourseId);
                        var enrollment = new Enrollment {
                            Student = student,
                            Course = context.Courses.Where(c => c.Id == randomCourseId).Single(),
                            EnrollmentDate = DateTime.UtcNow.AddDays(-new Random().Next(10,30))
                        };
                        context.Enrollments.Add(enrollment);
                    }
                }
                context.SaveChanges();
            }
            catch (Exception ex) {
                string message = ex.ToString();
                throw ex;
            }
        }

        private string RandomString(int v) {
            Random _rng = new Random((int)DateTime.Now.Ticks);
            string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            char[] buffer = new char[v];
            for (int i = 0; i < v; i++) {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        static string[] studentNames =
        {
            "Taiseer,Joudeh,Male,hotmail.com",
            "Hasan,Ahmad,Male,mymail.com",
            "Moatasem,Ahmad,Male,outlook.com",
            "Salma,Tamer,Female,outlook.com",
            "Ahmad,Radi,Male,gmail.com",
            "Bill,Gates,Male,yahoo.com",
            "Shareef,Khaled,Male,gmail.com",
            "Aram,Naser,Male,gmail.com",
            "Layla,Ibrahim,Female,mymail.com",
            "Rema,Oday,Female,hotmail.com",
            "Fikri,Husein,Male,gmail.com",
            "Zakari,Husein,Male,outlook.com",
            "Taher,Waleed,Male,mymail.com",
            "Tamer,Wesam,Male,yahoo.com",
            "Khaled,Hasaan,Male,gmail.com",
            "Asaad,Ibrahim,Male,hotmail.com",
            "Tareq,Nassar,Male,outlook.com",
            "Diana,Lutfi,Female,outlook.com",
            "Tamara,Malek,Female,gmail.com",
            "Arwa,Kamal,Female,yahoo.com",
            "Jana,Ahmad,Female,yahoo.com",
            "Nisreen,Tamer,Female,gmail.com",
            "Noura,Ahmad,Female,outlook.com"
        };

        static string[] tutorNames =
        {
            "Ahmad,Joudeh,Male,outlook.com",
            "Taiseer,Ahmad,Male,mymail.com",
            "Taymour,Wardi,Male,mymail.com",
            "Kareem,Ismail,Male,outlook.com",
            "Iyad,Radi,Male,gmail.com",
            "Ramdan,Ahmad,Male,gmalil.com",
            "Shareef,Khaled,Male,gmail.com",
            "Ibrahim,Naser,Male,outlook.com",
            "Layla,Ibrahim,Female,mymail.com",
            "Nisreen,Wesam,Female,hotmail.com"
        };

        static string[] subjects =
        {
            "History",
            "Science",
            "Geography",
            "English",
            "Commerce",
            "Computing",
            "Human Resource",
            "Mathematics",
            "Music",
            "Personal Development"
        };

        static IList<CoursesSeed> CoursesSeedData = new List<CoursesSeed>
        {
            new CoursesSeed {SubjectID =1, CourseName="History Teaching Methods 1"},
            new CoursesSeed {SubjectID =1, CourseName="History Teaching Methods 2"},
            new CoursesSeed {SubjectID =1, CourseName="History Teaching Methods 3"},

            new CoursesSeed {SubjectID =2, CourseName="Professional Experience 1 (Mathematics/Science)"},
            new CoursesSeed {SubjectID =2, CourseName="Professional Experience 2 (Mathematics/Science)"},
            new CoursesSeed {SubjectID =2, CourseName="Professional Experience 3 (Mathematics/Science)"},

            new CoursesSeed {SubjectID =3, CourseName="Geography Teaching Methods 1"},
            new CoursesSeed {SubjectID =3, CourseName="Geography Teaching Methods 2"},
            new CoursesSeed {SubjectID =3, CourseName="Geography Teaching Methods 3"},

            new CoursesSeed {SubjectID =4, CourseName="English Education 1"},
            new CoursesSeed {SubjectID =4, CourseName="English Education 2"},
            new CoursesSeed {SubjectID =4, CourseName="English Education 3"},
            new CoursesSeed {SubjectID =4, CourseName="English Teaching Methods 1"},
            new CoursesSeed {SubjectID =4, CourseName="English Teaching Methods 2"},

            new CoursesSeed {SubjectID =5, CourseName="Commerce, Business Studies and Economics Teaching Methods 1"},
            new CoursesSeed {SubjectID =5, CourseName="Commerce, Business Studies and Economics Teaching Methods 2"},
            new CoursesSeed {SubjectID =5, CourseName="Commerce, Business Studies and Economics Teaching Methods 3"},

            new CoursesSeed {SubjectID =6, CourseName="Computing Studies Teaching Methods 1"},
            new CoursesSeed {SubjectID =6, CourseName="Computing Studies Teaching Methods 2"},
            new CoursesSeed {SubjectID =6, CourseName="Computing Studies Teaching Methods 3"},

            new CoursesSeed {SubjectID =7, CourseName="Human Resource Development in Organisations"},
            new CoursesSeed {SubjectID =7, CourseName="Human Resources and Organisational Development"},

            new CoursesSeed {SubjectID =8, CourseName="Mathematics Teaching and Learning 1"},
            new CoursesSeed {SubjectID =8, CourseName="Mathematics Teaching and Learning 2"},
            new CoursesSeed {SubjectID =8, CourseName="Mathematics Teaching Methods 1"},
            new CoursesSeed {SubjectID =8, CourseName="Mathematics Teaching Methods 2"},

            new CoursesSeed {SubjectID =9, CourseName="Music Study 1"},
            new CoursesSeed {SubjectID =9, CourseName="Music Therapy 1"},
            new CoursesSeed {SubjectID =9, CourseName="Music, Movement and Dance"},

            new CoursesSeed {SubjectID =10, CourseName="Personal Development, Health and Physical Education 1"},
            new CoursesSeed {SubjectID =10, CourseName="Personal Development, Health and Physical Education Teaching Methods 1"},
            new CoursesSeed {SubjectID =10, CourseName="Personal Development, Health and Physical Education Teaching Methods 2"}
        };


        class CoursesSeed {
            public int SubjectID { get; set; }
            public string CourseName { get; set; }
        }
    }
}