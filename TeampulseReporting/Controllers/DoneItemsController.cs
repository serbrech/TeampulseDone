using System;
using System.Collections.Generic;
using System.Globalization;
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
            TempData.Add("IterationName", iteration.Name);
            return RedirectToAction("Date", new { fromDate = iteration.StartDate.ToShortDateString(), toDate = iteration.EndDate.ToShortDateString() });
        }

        public ActionResult Date(string fromDate, string toDate)
        {
            DateTime from;
            DateTime.TryParse(fromDate, out from);
            DateTime to;
            DateTime.TryParse(toDate, out to);
            var doneProblems = GetDoneProblemsByDate(from, to);
            var doneStories = GetDoneStoriesByDate(from, to);

            var iterationName = TempData["IterationName"] as string ?? fromDate + " - " + toDate;
            var viewModel = new DoneItemsViewModel()
            {
                IterationName = iterationName,
                DoneProblems = doneProblems.ToList(),
                DoneStories = doneStories.ToList()
            };

            return View("Index", viewModel);
        }

        private IEnumerable<DoneStory> GetDoneStoriesByDate(DateTime startDate, DateTime endDate)
        {
            var stories = from s in Context.Story.Include("Area")
                          where s.Status == "Done"
                          select s;

            var storyAudits = GetTableAudit("Story", startDate, endDate);

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
                                      PriorityClassification = s.PriorityClassification,
                                      TeampulseId = s.StoryID
                                  };
            return doneStories.AsEnumerable().Distinct(new DoneItemsComparer<DoneStory>());
        }

        private IEnumerable<DoneProblem> GetDoneProblemsByDate(DateTime startDate, DateTime endDate)
        {
            var problems = from p in Context.Problem.Include("Area")
                           //where p.CreatedSystemID
                           where p.Status == "Done"
                           select p;
            var problemAudits = GetTableAudit("Problem", startDate, endDate);

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
                                       TeampulseId = p.ProblemID
                                   };
            return doneProblems.AsEnumerable().Distinct(new DoneItemsComparer<DoneProblem>());
        }

        private IQueryable<Audit> GetTableAudit(string tableName, DateTime startdate, DateTime endDate)
        {
            return from a in Context.Audit
                   where a.TableName == tableName
                         && a.FieldName == "Status"
                         && a.NewValue == "Done"
                         && a.ModifiedDateUtc >= startdate
                         && a.ModifiedDateUtc <= endDate
                   select a;
        }


    }

    public class DoneItemsComparer<T> : IEqualityComparer<T> where T:DoneItem{
        public bool Equals(T x, T y)
        {
            return x.TeampulseId == y.TeampulseId;
        }

        public int GetHashCode(T obj)
        {
            return obj.TeampulseId;
        }
    }
}
