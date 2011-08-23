using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeampulseReporting.Data;
using TeampulseReporting.Models;

namespace TeampulseReporting.Controllers
{
    public class DoneItemsController : ControllerBase
    {
        //
        // GET: /Items/

        public ActionResult Index(int id)
        {
            var iteration = Context.Iteration.Where(it => it.IterationID == id).SingleOrDefault();

            if(iteration == null) return new HttpNotFoundResult("No such iteration, use the back button of your navigator");

            var problems = from p in Context.Problem.Include("Area")
                           where p.Status == "Done"
                           select p;

            var doneProblems = from a in Context.Audit
                               where a.TableName == "Problem"
                               && a.FieldName == "Status"
                               && a.NewValue == "Done"
                               && a.ModifiedDateUtc >= iteration.StartDate
                               && a.ModifiedDateUtc <= iteration.EndDate
                               join p in problems on a.PrimaryKeyValue equals p.ProblemID
                               orderby a.ModifiedDateUtc descending
                               select new DoneProblem
                                   {
                                       DoneDate = a.ModifiedDateUtc,
                                       CreationDate = p.CreatedDateUtc,
                                       DoneBy = a.ModifiedBy,
                                       ProblemName = p.Name,
                                       ProblemDescription = p.DescriptionPlainText,
                                       ProblemPriority = p.Priority,
                                       AreaName = p.Area.Name,
                                   };
            var viewModel = new DoneItemsViewModel()
                {
                    DoneProblems = doneProblems.ToList()
                };

            return View(viewModel);
        }

    }
}
