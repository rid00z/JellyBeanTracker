using System;
using Xamarin.Forms;

namespace JellyBeanTracker.Mobile.Pages
{
    public class BaseNav : NavigationPage
    {
        public BaseNav (Page page) : base(page)
        {
        }

        protected override void OnParentSet ()
        {

        }
    }
}

