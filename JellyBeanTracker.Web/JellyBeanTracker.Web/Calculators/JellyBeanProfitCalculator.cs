using System;
using System.Linq;
using JellyBeanTracker.Shared.DisplayModels;

namespace JellyBeanTracker.Shared.Calculators
{
    public class JellyBeanProfitCalculator
    {
        IDataSource _dataSource;

        public JellyBeanProfitCalculator (IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public ProfitReportModel CalculateProfit()
        {
            var jellyBeanValues = _dataSource.GetJellyBeanValues ().Result.ToList();
            var myJellyBeans = _dataSource.GetMyJellyBeans ().Result.ToList();

            decimal totalInvested = 0;
            decimal loses = 0;
            decimal gains = 0;
            decimal totalOutcome = 0;

            foreach (var myJellyBean in myJellyBeans) {
                var jellyBeanValue = jellyBeanValues.FirstOrDefault (o => o.Name == myJellyBean.JellyBeanName);
                var values = jellyBeanValue.Values;
                if (jellyBeanValue != null && values.Count() > 0) {
                    for (int i = 0; i < values.Count(); i++) {
                        decimal currentVal = values [i];
                        if (i == 0) {
                            totalInvested = currentVal * myJellyBean.TotalBeans;
                        } else {
                            var currentTotal = currentVal * myJellyBean.TotalBeans;
                            if (currentTotal > totalInvested)
                                gains += currentTotal - totalInvested;
                            else
                                loses += totalInvested - currentTotal;
                        }
                        if (i == (values.Count()-1))
                        {
                            totalOutcome = currentVal * myJellyBean.TotalBeans;
                        }
                    }
                }
            }

            return new ProfitReportModel
            {
                Gains = gains,
                Loses = loses,
                TotalInvested = totalInvested,
                TotalOutcome = totalOutcome
            };
        }
    }
}

