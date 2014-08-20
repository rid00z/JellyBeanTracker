using System;
using Xamarin.Forms;
using JellyBeanTracker.Mobile.PageModels;
using Xamarin.Forms.Labs.Controls;
using JellyBeanTracker.Mobile.CustomViews;

namespace JellyBeanTracker.Mobile.Pages
{
    public class ManageGraphPage : ContentPage
    {
        public ManageGraphPageModel PageModel { get; set; }

        public ManageGraphPage ()
        {
            Title = "Manage Graph";
            ToolbarItems.Add(new ToolbarItem("Done", null, () => {
                PageModel.Done.Execute(null);
            }));
            ToolbarItems.Add(new ToolbarItem("Cancel", null, () => {
                PageModel.Cancel.Execute(null);
            }));
        }

        public void Init()
        {
            var list = new EditableListView<Object> ();
            list.SetBinding (ListView.ItemsSourceProperty, "JellyBeanGraphData");
            list.ViewType = typeof(ManageGraphCustomView);
            if (PageModel.JellyBeanGraphData != null)
                list.Source = PageModel.JellyBeanGraphData;
            Content = list;
        }
    }
}

