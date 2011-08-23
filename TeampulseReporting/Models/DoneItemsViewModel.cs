using System.Collections.Generic;
using TeampulseReporting.Controllers;

namespace TeampulseReporting.Models
{
    public class DoneItemsViewModel {
        public string IterationName { get; set; }
        public IEnumerable<DoneProblem> DoneProblems { get; set; }
    }
}