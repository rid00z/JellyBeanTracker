using System;
using JellyBeanTracker.Shared;
using SQLite.Net;
using JellyBeanTracker.Mobile.Dependancies;
using JellyBeanTracker.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JellyBeanTracker.Mobile.Services
{
    public class LocalDataSource : IDataSource 
    {
        SQLiteConnection _sqliteConnection;

        public LocalDataSource ()
        {
            _sqliteConnection = Xamarin.Forms.DependencyService.Get<ISQLiteFactory> ().GetConnection("app.db");
            CreateTable ();
        }

        void CreateTable ()
        {
            _sqliteConnection.CreateTable<JellyBeanValue> ();
            _sqliteConnection.CreateTable<MyJellyBean> ();
        }

        public Task<IEnumerable<JellyBeanValue>> GetJellyBeanValues ()
        {
            return Task.FromResult((IEnumerable<JellyBeanValue>)_sqliteConnection.Table<JellyBeanValue> ());
        }

        public Task<IEnumerable<MyJellyBean>> GetMyJellyBeans ()
        {
            return Task.FromResult((IEnumerable<MyJellyBean>)_sqliteConnection.Table<MyJellyBean> ());
        }

    }
}

