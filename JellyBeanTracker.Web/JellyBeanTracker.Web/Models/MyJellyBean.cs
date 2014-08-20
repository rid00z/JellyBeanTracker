using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace JellyBeanTracker.Web.Models
{
    public class MyJellyBean
    {
        public Guid Id { get; set; }

        [DisplayName("Jelly Bean Name")]
        public string JellyBeanName { get; set; }

        [DisplayName("Total Beans")]
        public int TotalBeans { get; set; }
    }
}