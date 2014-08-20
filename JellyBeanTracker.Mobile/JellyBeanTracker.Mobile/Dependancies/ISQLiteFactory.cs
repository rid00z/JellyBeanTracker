using System;
using SQLite.Net;

namespace JellyBeanTracker.Mobile.Dependancies
{
    public interface ISQLiteFactory
    {
        SQLiteConnection GetConnection(string fileName);
    }
}

