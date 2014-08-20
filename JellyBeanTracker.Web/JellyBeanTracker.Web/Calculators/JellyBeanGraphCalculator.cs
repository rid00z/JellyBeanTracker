using JellyBeanTracker.Web.DisplayModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JellyBeanTracker.Shared.Calculators
{
    public class JellyBeanGraphCalculator
    {
        IDataSource _datasource;
        public JellyBeanGraphCalculator (IDataSource datasource)
        {
            _datasource = datasource;
        }

        public IEnumerable<JellyBeanGraphData> GetGraphData()
        {
            return _datasource.GetJellyBeanValues().Result
                        .Select(o => new
                            JellyBeanGraphData 
                            {
                                name = o.Name,
                                color = o.Name,
                                data = new List<decimal> { o.Jan, o.Feb, o.Mar, o.Apr, o.May, o.Jun, o.Jul, o.Aug, o.Sep, o.Oct, o.Nov, o.Dec }
                            });
        }                    
    }
}

