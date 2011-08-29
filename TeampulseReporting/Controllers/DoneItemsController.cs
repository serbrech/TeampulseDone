using System;
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
        public ActionResult Index(int id)
        {
            var iteration = Context.Iteration.Where(it => it.IterationID == id).SingleOrDefault();

            if (iteration == null) return new HttpNotFoundResult("No such iteration, use the back button of your navigator");

            var doneProblems = GetDoneProblems(iteration);
            var doneStories = GetDoneStories(iteration);

            var viewModel = new DoneItemsViewModel()
                {
                    IterationName = iteration.Name,
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

            var storyAudits = GetTableAudit("Story", iteration.StartDate, iteration.EndDate);

            var doneStories = from a in storyAudits
                              join s in stories on a.PrimaryKeyValue equals s.StoryID
                              orderby a.ModifiedDateUtc descending
                              select new DoneStory
                              {
                                  DoneDate = a.ModifiedDateUtc,
                                  CreationDate = s.CreatedDateUtc,
                                  DoneBy = s.AssignedTo,
                                  Name = s.Name,
                                  Description = s.Description,
                                  Priority = s.PriorityID,
                                  AreaName = s.Area.Name,
                                  Points = s.Points,
                                  PriorityClassification = s.PriorityClassification
                              };
            return doneStories;
        }

        private IQueryable<DoneProblem> GetDoneProblems(Iteration iteration)
        {
            var problems = from p in Context.Problem.Include("Area")
                           where p.Status == "Done"
                           select p;

            var problemAudits = GetTableAudit("Problem", iteration.StartDate, iteration.EndDate);

            var doneProblems = from a in problemAudits
                               join p in problems on a.PrimaryKeyValue equals p.ProblemID
                               orderby a.ModifiedDateUtc descending
                               select new DoneProblem
                                   {
                                       DoneDate = a.ModifiedDateUtc,
                                       CreationDate = p.CreatedDateUtc,
                                       DoneBy = p.AssignedTo,
                                       Name = p.Name,
                                       Description = p.DescriptionPlainText,
                                       Priority = p.Priority,
                                       AreaName = p.Area.Name,
                                   };
            return doneProblems;
        }

        private IQueryable<Audit> GetTableAudit(string tableName, DateTime startdate, DateTime endDate)
        {
            return from a in Context.Audit
                   where a.TableName == tableName
                         && a.FieldName == "Status"
                         && a.NewValue == "Done"
                         &&  a.ModifiedDateUtc >= startdate
                         && a.ModifiedDateUtc <= endDate
                   select a;
        }
    }
}
