using JellyBeanTracker.Web.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JellyBeanTracker.Shared
{
    public interface IDataSource
    {
        Task<IEnumerable<JellyBeanValue>> GetJellyBeanValues ();
        Task<IEnumerable<MyJellyBean>> GetMyJellyBeans ();
    }
}

