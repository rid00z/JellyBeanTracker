using System;
using JellyBeanTracker.Mobile.Services;
using JellyBeanTracker.Web.Models;
using System.Collections.Generic;
using PropertyChanged;
using System.Collections.ObjectModel;

namespace JellyBeanTracker.Mobile.PageModels
{
    [ImplementPropertyChanged]
    public class MyJellyBeansListModel : BasePageModel
    {
        public ObservableCollection<Object> MyJellyBeans { get; set; }

        IDataSourceFactory _dataSourceFactory;
        public MyJellyBeansListModel (IDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;
        }

        public void Init(object initData)
        {
            MyJellyBeans = new ObservableCollection<Object>(_dataSourceFactory.GetDataSource ().GetMyJellyBeans ().Result);
        }

    }
}

