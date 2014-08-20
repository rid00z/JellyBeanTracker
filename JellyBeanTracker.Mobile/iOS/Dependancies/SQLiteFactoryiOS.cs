using Xamarin.Forms;
using JellyBeanTracker.Mobile.iOS.Dependancies;
using JellyBeanTracker.Mobile.Dependancies;
using System;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;

[assembly: Dependency(typeof(SQLiteFactoryiOS))]

namespace JellyBeanTracker.Mobile.iOS.Dependancies
{
    public class SQLiteFactoryiOS : ISQLiteFactory
    {
        public SQLite.Net.SQLiteConnection GetConnection (string fileName)
        {
            var path = System.IO.Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), fileName);
            return new SQLiteConnection (new SQLitePlatformIOS(), path);
        }
    }
}

