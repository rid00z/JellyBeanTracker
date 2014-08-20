using System;
using System.Collections.Generic;
using System.Linq;

namespace JellyBeanTracker.Web.Models
{
    public class SyncContainer
    {
        public List<JellyBeanValue> JellyBeanValues { get; set; }
        public List<MyJellyBean> MyJellyBeans { get; set; }
    }
}