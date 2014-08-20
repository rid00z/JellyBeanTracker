using System;
using SQLite.Net;
using System.IO;
using SQLite.Net.Platform.XamarinAndroid;
using Xamarin.Forms;
using JellyBeanTracker.Mobile.Dependancies;
using JellyBeanTracker.Mobile.Android.Dependancies;

[assembly: Dependency(typeof(SQLiteFactoryDroid))]

namespace JellyBeanTracker.Mobile.Android.Dependancies
{
    public class SQLiteFactoryDroid : ISQLiteFactory
    {
        public SQLiteFactoryDroid ()
        {
        }

        public SQLiteConnection GetConnection (string fileName)
        {
            var path = Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), fileName);
            return new SQLiteConnection (new SQLitePlatformAndroid (), path);
        }
    }
}
