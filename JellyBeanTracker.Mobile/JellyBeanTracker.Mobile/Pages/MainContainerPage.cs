using System;
using Xamarin.Forms;
using JellyBeanTracker.Mobile.PageModels;

namespace JellyBeanTracker.Mobile.Pages
{
    public class MainContainerPage : TabbedPage, ITabbedNavigation
    {
        public MainContainerPage ()
        {
            var jellyBeanListPage = new BaseNav (BasePageModel.ResolveViewModel<JellyBeanListPageModel> (null));
            jellyBeanListPage.Title = "Jelly Bean Prices";
            jellyBeanListPage.Icon = "Icons/peanuts/peanuts.png";
            var graphPage = new BaseNav (BasePageModel.ResolveViewModel<GraphPageModel> (null));
            graphPage.Title = "Price Graph";
            graphPage.Icon = "Icons/line_chart/line_chart.png";
            var myBeansPage = new BaseNav (BasePageModel.ResolveViewModel<MyJellyBeansListModel> (null));
            myBeansPage.Title = "My JellyBeans";
            myBeansPage.Icon = "Icons/purchase_order/purchase_order.png";
            var myReportPage = new BaseNav (BasePageModel.ResolveViewModel<JellyBeanProfitPageModel> (null));
            myReportPage.Title = "Profit Report";
            myReportPage.Icon = "Icons/money_bag/money_bag.png";

            Children.Add (jellyBeanListPage);
            Children.Add (graphPage);
            Children.Add (myBeansPage);
            Children.Add (myReportPage);
        }

        public void PushView (BasePageModel viewModelToPush, Page pageToPush, bool model)
        {
            if (model)
                this.CurrentPage.Navigation.PushModalAsync (pageToPush);
            else
                this.CurrentPage.Navigation.PushAsync (pageToPush);
        }

        public void PopView (bool model)
        {
            if (model)
                this.CurrentPage.Navigation.PopModalAsync ();
            else
                this.CurrentPage.Navigation.PopAsync ();
        }
    }
}



