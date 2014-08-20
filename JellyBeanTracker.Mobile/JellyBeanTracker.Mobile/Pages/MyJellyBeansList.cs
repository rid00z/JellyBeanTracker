using System;
using Xamarin.Forms;
using JellyBeanTracker.Mobile.PageModels;
using Xamarin.Forms.Labs.Controls;
using JellyBeanTracker.Mobile.CustomViews;

namespace JellyBeanTracker.Mobile.Pages
{
    public class MyJellyBeansList : ContentPage
    {
        public MyJellyBeansListModel PageModel { get; set; }

        public MyJellyBeansList ()
        {
            Title = "My JellyBeans";
        }

        public void Init()
        {
            if (Device.OS == TargetPlatform.iOS) {
                var list = new EditableListView<Object> ();
                list.SetBinding (EditableListView<Object>.SourceProperty, "MyJellyBeans");
                list.ViewType = typeof(MyJellyBeanListViewCell);
                list.CellHeight = 60;
                list.AddRowCommand = new Command (() => {
                    this.DisplayAlert ("Sorry", "Not implemented yet!", "OK");
                });

                if (PageModel.MyJellyBeans != null)
                    list.Source = PageModel.MyJellyBeans;
                Content = list;
            } else {
                var list = new ListView();
                list.SetBinding (ListView.ItemsSourceProperty, "MyJellyBeans");
                var celltemp = new DataTemplate(typeof(TextCell));
                celltemp.SetBinding (TextCell.TextProperty, "JellyBeanName");
                celltemp.SetBinding (TextCell.DetailProperty, "TotalBeans");
                list.ItemTemplate = celltemp;
                if (PageModel.MyJellyBeans != null)
                    list.ItemsSource = PageModel.MyJellyBeans;
                Content = list;
            }

            ToolbarItems.Add(new ToolbarItem("Add", "", () => {
                this.DisplayAlert("Sorry", "Not implemented yet!", "OK");
            }));
        }
    }
}

