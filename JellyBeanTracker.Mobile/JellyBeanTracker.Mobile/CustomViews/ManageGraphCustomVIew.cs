using System;
using Xamarin.Forms;

namespace JellyBeanTracker.Mobile.CustomViews
{
    public class ManageGraphCustomView : ContentView
    {
        public ManageGraphCustomView ()
        {
            var label = new Label ();
            label.SetBinding (Label.TextProperty, "name");
            label.WidthRequest = 200;
            //label.Text = "this is a big test";

            var showSwitch = new Switch();
            //showSwitch.SetBinding (Switch.IsToggledProperty, "show");
            Content = new StackLayout {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(20, 10, 20, 10),
                Children = { label  },
            };

        }


    }
}

