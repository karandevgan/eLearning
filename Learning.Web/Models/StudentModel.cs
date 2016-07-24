﻿using System;
using System.Collections.Generic;

namespace Learning.Web.Models {
    public class StudentModel: StudentBaseModel {
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public IEnumerable<EnrollmentModel> Enrollments { get; set; }
    }
}