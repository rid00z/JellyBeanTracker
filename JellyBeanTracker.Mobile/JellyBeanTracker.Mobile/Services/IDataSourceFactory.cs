using System;
using JellyBeanTracker.Shared;

namespace JellyBeanTracker.Mobile.Services
{
    public interface IDataSourceFactory
    {
        IDataSource GetDataSource();
    }
}

