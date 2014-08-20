using System;
using Xamarin.Forms;

namespace JellyBeanTracker.Mobile.CustomViews
{
    public class MyJellyBeanListViewCell : ContentView
    {
        public MyJellyBeanListViewCell ()
        {
            var label = new Label ();
            label.SetBinding (Label.TextProperty, "JellyBeanName");
            label.Font = Font.SystemFontOfSize (20);
            label.TextColor = Color.FromHex ("0E68C4");
            label.VerticalOptions = LayoutOptions.FillAndExpand;
            label.HorizontalOptions = LayoutOptions.FillAndExpand;

            var labelReadValues = new Label ();
            labelReadValues.SetBinding (Label.TextProperty, "TotalBeans");
            labelReadValues.Font = Font.SystemFontOfSize (20);
            labelReadValues.VerticalOptions = LayoutOptions.FillAndExpand;
            labelReadValues.HorizontalOptions = LayoutOptions.FillAndExpand;

            Content = new StackLayout {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(20, 20, 20, 10),
                Children = { label, labelReadValues  },
            };

        }


    }
}

