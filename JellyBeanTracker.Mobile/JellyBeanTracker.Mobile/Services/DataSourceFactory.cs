using System;

namespace JellyBeanTracker.Mobile.Services
{
    public class DataSourceFactory : IDataSourceFactory
    {
        public DataSourceFactory ()
        {
        }

        public JellyBeanTracker.Shared.IDataSource GetDataSource ()
        {
            if (App.IsOffline)
                return new LocalDataSource ();
            else
                return new RemoteDataSource ();
        }
    }
}

