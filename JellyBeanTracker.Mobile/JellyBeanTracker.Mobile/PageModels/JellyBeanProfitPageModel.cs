using System;
using JellyBeanTracker.Mobile.Services;
using JellyBeanTracker.Web.DisplayModels;
using JellyBeanTracker.Shared.Calculators;
using JellyBeanTracker.Shared.DisplayModels;

namespace JellyBeanTracker.Mobile.PageModels
{
    public class JellyBeanProfitPageModel : BasePageModel
    {
        IDataSourceFactory _dataSourceFactory;
        public ProfitReportModel ProfitData { get; set; }

        public JellyBeanProfitPageModel (IDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;
        }

        public void Init(object initData)
        {
            ReloadData ();
        }

        void ReloadData()
        {
            var dataSource = _dataSourceFactory.GetDataSource ();
            ProfitData = new JellyBeanProfitCalculator (dataSource).CalculateProfit ();
            if (DataLoaded != null)
                DataLoaded ();
        }

        public event Action DataLoaded;

    }
}

