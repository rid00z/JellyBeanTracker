using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JellyBeanTracker.Web.Data
{
    public class JellyBeanInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<JellyBeanDataContext>
    {
        protected override void Seed(JellyBeanDataContext context)
        {
            base.Seed(context);

            context.JellyBeanValues.Add(new Models.JellyBeanValue
            {
                Id = Guid.NewGuid(),
                Name = "Blue",
                Jan = 55.3M,
                Feb = 67.3M,
                Mar = 78.3M,
                May = 56M,
                Apr = 97.3M,
                Jun = 110.3M,
                Jul = 34.3M,
                Aug = 58.3M,
                Sep = 85.3M,
                Oct = 99.3M,
                Nov = 50.3M,
                Dec = 25.3M                
            });

            context.JellyBeanValues.Add(new Models.JellyBeanValue
            {
                Id = Guid.NewGuid(),
                Name = "Green",
                Jan = 54.3M,
                Feb = 89.3M,
                Mar = 62.3M,
                Apr = 78.3M,
                May = 56M,
                Jun = 71.3M,
                Jul = 82.3M,
                Aug = 95.3M,
                Sep = 78.3M,
                Oct = 54.3M,
                Nov = 98.3M,
                Dec = 92.3M
            });

            context.JellyBeanValues.Add(new Models.JellyBeanValue
            {
                Id = Guid.NewGuid(),
                Name = "Yellow",
                Jan = 45.3M,
                Feb = 65.3M,
                Mar = 99.3M,
                Apr = 72.3M,
                May = 56M,
                Jun = 75.3M,
                Jul = 85.3M,
                Aug = 36.3M,
                Sep = 78.3M,
                Oct = 82.3M,
                Nov = 65.3M,
                Dec = 52.3M
            });

            context.JellyBeanValues.Add(new Models.JellyBeanValue
            {
                Id = Guid.NewGuid(),
                Name = "Purple",
                Jan = 85.3M,
                Feb = 96.3M,
                Mar = 74.3M,
                Apr = 75.3M,
                May = 56M,
                Jun = 91.3M,
                Jul = 41.3M,
                Aug = 52.3M,
                Sep = 74.3M,
                Oct = 85.3M,
                Nov = 96.3M,
                Dec = 96.3M
            });

            context.MyJellyBeans.Add(new Models.MyJellyBean
            {
                Id = Guid.NewGuid(),
                JellyBeanName = "Blue",
                TotalBeans = 700
            });

            context.MyJellyBeans.Add(new Models.MyJellyBean
            {
                Id = Guid.NewGuid(),
                JellyBeanName = "Green",
                TotalBeans = 523
            });

            context.MyJellyBeans.Add(new Models.MyJellyBean
            {
                Id = Guid.NewGuid(),
                JellyBeanName = "Yellow",
                TotalBeans = 900
            });

            context.MyJellyBeans.Add(new Models.MyJellyBean
            {
                Id = Guid.NewGuid(),
                JellyBeanName = "Purple",
                TotalBeans = 200
            });

        }

    }
}