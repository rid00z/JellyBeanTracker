using System;
using JellyBeanTracker.Mobile.Services;

namespace JellyBeanTracker.Mobile.PageModels
{
    public class JellyBeanEditPageModel : BasePageModel
    {
        IDataSourceFactory _dataSourceFactory;
        public JellyBeanEditPageModel (IDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;
        }

        public void Init(object initData)
        {

        }

    }
}

