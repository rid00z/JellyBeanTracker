using System;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;

namespace JellyBeanTracker.Web.DisplayModels
{
    [ImplementPropertyChanged]
    public class JellyBeanGraphData
    {
        public JellyBeanGraphData()
        {
            show = true;
        }
        public string name { get; set; }
        public string color { get; set; }
        public List<decimal> data { get; set; }
        public bool show { get; set; }
    }
}