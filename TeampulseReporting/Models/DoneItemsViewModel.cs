using System.Collections;
using System.Collections.Generic;
using TeampulseReporting.Controllers;

namespace TeampulseReporting.Models
{
    public class DoneItemsViewModel {
        public DoneItemsViewModel()
        {
            DoneProblems = new List<DoneProblem>();
            DoneStories = new List<DoneStory>();
        }
        public string IterationName { get; set; }
        public IEnumerable<DoneProblem> DoneProblems { get; set; }
        public IEnumerable<DoneStory> DoneStories { get; set; }
    }
}