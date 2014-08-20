using System;
using Xamarin.Forms;

namespace JellyBeanTracker.Mobile.CustomViews
{
    public class JellyBeanListViewCell : ContentView
    {
        public JellyBeanListViewCell ()
        {
            var label = new Label ();
            label.SetBinding (Label.TextProperty, "Name");
            label.Font = Font.SystemFontOfSize (20);
            label.TextColor = Color.FromHex ("0E68C4");
            label.VerticalOptions = LayoutOptions.FillAndExpand;
            label.HorizontalOptions = LayoutOptions.FillAndExpand;

            var labelReadValues = new Label ();
            labelReadValues.SetBinding (Label.TextProperty, "ReadableValues");
            labelReadValues.Font = Font.SystemFontOfSize (14);
            labelReadValues.VerticalOptions = LayoutOptions.FillAndExpand;
            labelReadValues.HorizontalOptions = LayoutOptions.FillAndExpand;

            Content = new StackLayout {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Padding = new Thickness(20, 10, 20, 10),
                Children = { label, labelReadValues  },
            };

        }


    }
}

