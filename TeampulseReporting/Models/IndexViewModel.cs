using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeampulseReporting.Data;

namespace TeampulseReporting.Models
{
    public class IndexViewModel
    {
        public List<Iteration> Iterations { get; set; }

        public IndexViewModel(List<Iteration> iterations)
        {
            Iterations = iterations;
        }
    }
}