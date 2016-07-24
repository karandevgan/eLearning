using Learning.Data;
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
        public StudentsController(ILearningRepository repo) : base(repo) {

        }
    }
}
