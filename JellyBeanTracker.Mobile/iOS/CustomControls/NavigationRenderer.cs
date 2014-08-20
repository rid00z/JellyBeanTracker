using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using Xamarin.Forms.Platform.iOS;
using JellyBeanTracker.Mobile.Pages;

[assembly: Xamarin.Forms.ExportRenderer(typeof(BaseNav), typeof(JellyBeanTracker.Mobile.iOS.NavigationRenderer))]

namespace JellyBeanTracker.Mobile.iOS
{
    public class NavigationRenderer : Xamarin.Forms.Platform.iOS.NavigationRenderer
    {
        public NavigationRenderer()
        {
        }

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            base.PushViewController(viewController, animated);

            bool cancel = false;
            if (TopViewController.NavigationItem.RightBarButtonItems != null)
            {
                List<UIBarButtonItem> newRightBtns = new List<UIBarButtonItem> ();
                foreach (var barItem in TopViewController.NavigationItem.RightBarButtonItems)
                {
                    if (barItem.Title != null && barItem.Title.Contains("Cancel"))
                    {
                        cancel = true;
                        TopViewController.NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[] { barItem }, false);
                    }
                    else
                    {
                        newRightBtns.Add(barItem);
                    }
                }
                TopViewController.NavigationItem.SetRightBarButtonItems(newRightBtns.ToArray(), false);
            }

            if (!cancel) {
                var menuButton = new UIBarButtonItem (UIImage.FromBundle ("Icons/menu.png"), UIBarButtonItemStyle.Plain, (s, e) => {
                    Xamarin.Forms.MessagingCenter.Send<object> (this, "MenuButtonTapped");
                });
                TopViewController.NavigationItem.SetLeftBarButtonItems(new UIBarButtonItem[] { menuButton }, false);
            }

            TopViewController.NavigationController.NavigationBar.BackgroundColor = UIColor.FromRGB (52, 152, 219);
            TopViewController.NavigationController.NavigationBar.TintColor = UIColor.White;
            TopViewController.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB (52, 152, 219);

            TopViewController.NavigationController.NavigationBar.SetTitleTextAttributes(new UITextAttributes()
                {
                    TextColor = UIColor.White
                }); 
        }

    }
}

