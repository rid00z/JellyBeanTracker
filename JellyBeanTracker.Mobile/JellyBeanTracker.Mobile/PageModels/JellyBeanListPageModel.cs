using System;
using JellyBeanTracker.Mobile.Services;
using JellyBeanTracker.Web.Models;
using System.Collections.Generic;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace JellyBeanTracker.Mobile.PageModels
{
    [ImplementPropertyChanged]
    public class JellyBeanListPageModel : BasePageModel
    {
        IDataSourceFactory _dataSourceFactory;
        public ObservableCollection<Object> JellyBeanValues { get; set; }

        public JellyBeanListPageModel (IDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;
        }

        public void Init(object initData)
        {
            JellyBeanValues = new ObservableCollection<Object>(_dataSourceFactory.GetDataSource ().GetJellyBeanValues ().Result);
        }
    }
}

