using System;

namespace TeampulseReporting.Models
{
    public class DoneItem
    {
        public DateTime DoneDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string DoneBy { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public float? Priority { get; set; }
        public string AreaName { get; set; }
    }
}