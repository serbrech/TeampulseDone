
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeampulseReporting.Models;

namespace TeampulseReporting.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            var iterations = (from i in Context.Iteration
                              orderby i.IterationID descending 
                              select i).ToList();
            return View(new IndexViewModel(iterations));
        }

    }
}
