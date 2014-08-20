using System;
using Xamarin.Forms;
using JellyBeanTracker.Mobile.PageModels;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Services.Serialization;
using Newtonsoft.Json;

namespace JellyBeanTracker.Mobile.Pages 
{
    public class GraphPage : ContentPage
    {
        public GraphPageModel PageModel { get; set; }
        HybridWebView _hybridWebView;

        public GraphPage ()
        {
            Title = "Jellybean Prices Graph";
        }

        protected override void OnAppearing ()
        {
            if (PageModel.GraphData != null)
                PageDataLoaded ();

            PageModel.DataLoaded -= PageDataLoaded;
            PageModel.DataLoaded += PageDataLoaded;
            PageModel.PropertyChanged -= HandlePropertyChanged;
            PageModel.PropertyChanged += HandlePropertyChanged;
        }

        void HandlePropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        void PageDataLoaded ()
        {
            if (_hybridWebView == null) {
                _hybridWebView = new HybridWebView (new JsonNetJsonSerializer());
                _hybridWebView.HorizontalOptions = LayoutOptions.FillAndExpand;
                _hybridWebView.VerticalOptions = LayoutOptions.FillAndExpand;
                _hybridWebView.RegisterCallback ("EditVisible", (data) => {
                    PageModel.EditVisible.Execute(data);
                });
                _hybridWebView.RegisterNativeFunction ("GetGraphData", (input) => {
                    return PageModel.GetGraphData(input);
                });

                _hybridWebView.LoadFinished += (s, e) => {
                    string data = JsonConvert.SerializeObject (PageModel.GraphData).Replace ("\r\n", "");
                    _hybridWebView.InjectJavaScript ("JellyBeanTrackerApp.buildChartStr('" + data + "');");
                };
                Content = _hybridWebView;
            }

            var profitReportRazorView = new JellyBeanGraph ();// { Model = PageModel.GraphData }; 
            var html = profitReportRazorView.GenerateString ();
            _hybridWebView.LoadContent (html.Replace("\r\n", ""));

        }
    }
}

