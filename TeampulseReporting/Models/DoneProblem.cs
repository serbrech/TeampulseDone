using System;

namespace TeampulseReporting.Controllers
{
    public class DoneProblem
    {

        public DateTime DoneDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string DoneBy { get; set; }
        public string ProblemDescription { get; set; }
        public string ProblemName { get; set; }
        public float? ProblemPriority { get; set; }
        public string AreaName { get; set; }
    }
}