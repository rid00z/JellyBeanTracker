using System;
using Xamarin.Forms;
using JellyBeanTracker.Mobile.PageModels;
using Xamarin.Forms.Labs.Controls;
using JellyBeanTracker.Mobile.CustomViews;
using System.Collections.Generic;

namespace JellyBeanTracker.Mobile.Pages
{
    public class JellyBeanListPage : ContentPage
    {
        public JellyBeanListPageModel PageModel { get; set; }

        public JellyBeanListPage ()
        {
            Title = "Jelly Bean List";
        }

        public void Init()
        {
            if (Device.OS == TargetPlatform.iOS) {
                var list = new EditableListView<Object> ();
                list.SetBinding (EditableListView<Object>.SourceProperty, "JellyBeanValues");
                list.ViewType = typeof(JellyBeanListViewCell);
                list.CellHeight = 60;
                list.AddRowCommand = new Command (() => {
                    this.DisplayAlert ("Sorry", "Not implemented yet!", "OK");
                });

                if (PageModel.JellyBeanValues != null)
                    list.Source = PageModel.JellyBeanValues;
                Content = list;
            } else {
                var list = new ListView();
                list.SetBinding (ListView.ItemsSourceProperty, "JellyBeanValues");
                var celltemp = new DataTemplate(typeof(TextCell));
                celltemp.SetBinding (TextCell.TextProperty, "Name");
                celltemp.SetBinding (TextCell.DetailProperty, "ReadableValues");
                list.ItemTemplate = celltemp;
                if (PageModel.JellyBeanValues != null)
                    list.ItemsSource = PageModel.JellyBeanValues;
                Content = list;
            }

            ToolbarItems.Add(new ToolbarItem("Add", "", () => {
                this.DisplayAlert("Sorry", "Not implemented yet!", "OK");
            }));
        }

    }
}

