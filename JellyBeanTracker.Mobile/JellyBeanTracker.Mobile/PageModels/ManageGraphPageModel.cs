using System;
using JellyBeanTracker.Mobile.Services;
using Xamarin.Forms;
using Newtonsoft.Json;
using JellyBeanTracker.Web.DisplayModels;
using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace JellyBeanTracker.Mobile.PageModels
{
    [ImplementPropertyChanged]
    public class ManageGraphPageModel : BasePageModel
    {
        public ObservableCollection<Object> JellyBeanGraphData { get; set; }

        public ManageGraphPageModel ()
        {
        }

        public void Init(object initData)
        {
            JellyBeanGraphData = new ObservableCollection<Object>(JsonConvert.DeserializeObject<List<JellyBeanGraphData>> ((string)initData));
        }

        public Command Done
        {
            get {
                return new Command ((data) => {
                    PopViewModel(JellyBeanGraphData.Select(o => (JellyBeanGraphData)o).ToList(), true);
                });
            }
        }

        public Command Cancel
        {
            get {
                return new Command ((data) => {
                    PopViewModel(true);
                });
            }
        }
    }
}

