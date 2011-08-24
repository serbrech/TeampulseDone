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

            var doneProblems = GetDoneProblems(iteration);
            var doneStories = GetDoneStories(iteration);

            var viewModel = new DoneItemsViewModel()
                {
                    DoneProblems = doneProblems.ToList(),
                    DoneStories = doneStories.ToList()
                };

            return View(viewModel);
        }

        private IQueryable<DoneStory> GetDoneStories(Iteration iteration)
        {
            var stories = from s in Context.Story.Include("Area")
                           where s.Status == "Done"
                           select s;

            var doneStories = from a in Context.Audit
                               where a.TableName == "Story"
                                     && a.FieldName == "Status"
                                     && a.NewValue == "Done"
                                     && a.ModifiedDateUtc >= iteration.StartDate
                                     && a.ModifiedDateUtc <= iteration.EndDate
                               join p in stories on a.PrimaryKeyValue equals p.StoryID
                               orderby a.ModifiedDateUtc descending
                               select new DoneStory
                               {
                                   DoneDate = a.ModifiedDateUtc,
                                   CreationDate = p.CreatedDateUtc,
                                   DoneBy = a.ModifiedBy,
                                   Name = p.Name,
                                   Description = p.Description,
                                   Priority = p.PriorityID,
                                   AreaName = p.Area.Name,
                                   Points = p.Points,
                                   PriorityClassification = p.PriorityClassification
                               };
            return doneStories;
        }

        private IQueryable<DoneProblem> GetDoneProblems(Iteration iteration)
        {
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
                                       Name = p.Name,
                                       Description = p.DescriptionPlainText,
                                       Priority = p.Priority,
                                       AreaName = p.Area.Name,
                                   };
            return doneProblems;
        }
    }
}
