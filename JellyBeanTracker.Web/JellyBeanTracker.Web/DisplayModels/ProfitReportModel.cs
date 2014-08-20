using System;

namespace JellyBeanTracker.Shared.DisplayModels
{
    public class ProfitReportModel
    {
        public decimal TotalInvested { get; set; }
        public decimal Loses { get; set; }
        public decimal Gains { get; set; }
        public decimal TotalOutcome { get; set; }
    }
}

