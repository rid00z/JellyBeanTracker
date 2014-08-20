using System;
using JellyBeanTracker.Mobile.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Labs.Controls;
using Xamarin.Forms.Labs.Services.Serialization;

namespace JellyBeanTracker.Mobile.Pages
{
    public class JellyBeanProfitPage : ContentPage 
    {
        public JellyBeanProfitPageModel PageModel { get; set; }
        HybridWebView _hybridWebView;

        public JellyBeanProfitPage ()
        {
            Title = "Profit Report";
        }

        public void Init()
        {

        }

        protected override void OnAppearing ()
        {
            if (PageModel.ProfitData != null)
                PageDataLoaded ();

            PageModel.DataLoaded -= PageDataLoaded;
            PageModel.DataLoaded += PageDataLoaded;
        }

        void PageDataLoaded ()
        {
            if (_hybridWebView == null) {
                _hybridWebView = new HybridWebView (new SystemJsonSerializer());
                _hybridWebView.HorizontalOptions = LayoutOptions.FillAndExpand;
                _hybridWebView.VerticalOptions = LayoutOptions.FillAndExpand;

                Content = _hybridWebView;
            }

            var profitReportRazorView = new ProfitReport () { Model = PageModel.ProfitData }; 
            var html = profitReportRazorView.GenerateString ();
            _hybridWebView.LoadContent (html.Replace("\r\n", ""));
        }

    }
}

