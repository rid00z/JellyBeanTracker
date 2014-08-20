using System;
using Xamarin.Forms;
using JellyBeanTracker.Mobile.Pages;
using JellyBeanTracker.Mobile.Services;

namespace JellyBeanTracker.Mobile
{
    public class App
    {
        public static bool IsOffline = false;

        public static Page GetMainPage ()
        {	
            TinyIoC.TinyIoCContainer.Current.Register<IDataSourceFactory, DataSourceFactory> ().AsSingleton ();

            var tabbedPage = new MainContainerPage ();

            TinyIoC.TinyIoCContainer.Current.Register<ITabbedNavigation> (tabbedPage);

            return tabbedPage;
        }
    }
}

