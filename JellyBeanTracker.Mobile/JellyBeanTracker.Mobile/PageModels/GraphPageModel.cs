using System;
using JellyBeanTracker.Mobile.Services;
using PropertyChanged;
using JellyBeanTracker.Web.DisplayModels;
using JellyBeanTracker.Shared.Calculators;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace JellyBeanTracker.Mobile.PageModels
{
    [ImplementPropertyChanged]
    public class GraphPageModel : BasePageModel
    {
        IDataSourceFactory _dataSourceFactory;
        public IEnumerable<JellyBeanGraphData> GraphData { get; set; }
        public event Action DataLoaded;

        public GraphPageModel (IDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;
        }

        public void Init(object initData)
        {
            GraphData = new JellyBeanGraphCalculator (_dataSourceFactory.GetDataSource ()).GetGraphData();
        }

        public void ReverseInit(object data)
        {
            if (data is IEnumerable<JellyBeanGraphData>) {
                GraphData = data as IEnumerable<JellyBeanGraphData>;
            }
        }

        public Command<string> EditVisible
        {
            get {
                return new Command<string> ((data) => {
                    PushViewModel<ManageGraphPageModel>(data, true);
                });
            }
        }

        public Func<string, object[]> GetGraphData
        {
            get {
                return (data) => {
                    GraphData = new JellyBeanGraphCalculator (_dataSourceFactory.GetDataSource ()).GetGraphData();
                    return new object[] { GraphData };
                };
            }
        }

    }
}

