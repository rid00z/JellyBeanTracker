using JellyBeanTracker.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace JellyBeanTracker.Web.Data
{
    public class JellyBeanDataContext : DbContext
    {
        public JellyBeanDataContext()
            : base("JellyBeanDataContext")
        {
        }

        public DbSet<JellyBeanValue> JellyBeanValues { get; set; }
        public DbSet<MyJellyBean> MyJellyBeans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}