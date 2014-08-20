using System;
using JellyBeanTracker.Shared;
using System.Collections.Generic;
using JellyBeanTracker.Web.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JellyBeanTracker.Mobile.Dependancies;

namespace JellyBeanTracker.Mobile.Services
{
    public class RemoteDataSource : IDataSource 
    {
        static string HostBase = "http://192.168.0.120:49203/";
        public RemoteDataSource ()
        {
        }

        public async Task<IEnumerable<JellyBeanValue>> GetJellyBeanValues ()
        {
            return (await GetServerData ()).JellyBeanValues;
        }

        public async Task<IEnumerable<MyJellyBean>> GetMyJellyBeans ()
        {
            return (await GetServerData ()).MyJellyBeans;
        }

        async Task<SyncContainer> GetServerData()
        {
            HttpClient client = new HttpClient ();
            var result = client.GetStringAsync (HostBase + "/JellyBeans/GetAllData").Result;

            var syncContainer = JsonConvert.DeserializeObject<SyncContainer> (result);

            //cheap mans offline for this sample, put all into localstoage
            var sql = Xamarin.Forms.DependencyService.Get<ISQLiteFactory> ().GetConnection ("app.db");

            sql.CreateTable<MyJellyBean> ();
            sql.CreateTable<JellyBeanValue> ();

            if (sql.Table<MyJellyBean>().Count() < 1)
                sql.InsertAll (syncContainer.MyJellyBeans);

            if (sql.Table<JellyBeanValue>().Count() < 1)
                sql.InsertAll (syncContainer.JellyBeanValues);

            return syncContainer;
        }


    }
}

