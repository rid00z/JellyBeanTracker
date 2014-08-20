using JellyBeanTracker.Shared;
using JellyBeanTracker.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JellyBeanTracker.Web.Data
{
    public class DataContentDataSource : IDataSource
    {
        JellyBeanDataContext _dc;

        public DataContentDataSource(JellyBeanDataContext dc)
        {
            _dc = dc;
        }

        System.Threading.Tasks.Task<IEnumerable<JellyBeanValue>> IDataSource.GetJellyBeanValues()
        {
            return Task.FromResult <IEnumerable<JellyBeanValue>>(_dc.JellyBeanValues);
        }

        System.Threading.Tasks.Task<IEnumerable<MyJellyBean>> IDataSource.GetMyJellyBeans()
        {
            return Task.FromResult< IEnumerable<MyJellyBean>>(_dc.MyJellyBeans);
        }
    }
}