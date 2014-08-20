using System;
using Xamarin.Forms.Platform.iOS;
using MR;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.Foundation;
using Xamarin.Forms;
using System.Collections.Generic;
using JellyBeanTracker.Mobile.Pages;
using JellyBeanTracker.Mobile.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(MainContainerPage), typeof(SideMenuRenderer))]

namespace JellyBeanTracker.Mobile.iOS
{
    public class SideMenuRenderer : MRSideMenu, IVisualElementRenderer, IDisposable, IRegisterable 
    {
        public event EventHandler<VisualElementChangedEventArgs> ElementChanged;

        public void SetElement (VisualElement element)
        {
            VisualElement element2 = this.Element;
            this.Element = element;
            this.UpdateTitle ();
            this.OnElementChanged (new VisualElementChangedEventArgs (element2, element));
        }

        protected virtual void OnElementChanged (VisualElementChangedEventArgs e)
        {
            EventHandler<VisualElementChangedEventArgs> elementChanged = this.ElementChanged;
            if (elementChanged != null) {
                elementChanged (this, e);
            }

            _pages = new List<UIViewController>();

            foreach (var page in ContainerPage.Children)
            {
                var pageVC = page.CreateViewController();

                _pages.Add(pageVC);
            }

            var leftMenuViewController = new LeftMenuController(this);

            SetControllers(_pages[0], leftMenuViewController, null);
            BackgroundImage = UIImage.FromBundle ("stars.png");

            this.View.AccessibilityLabel = "Side Menu Renderer";

            Xamarin.Forms.MessagingCenter.Subscribe<object>(this, "MenuButtonTapped", (s) =>
                {
                    Toggle();
                });
        }

        private void UpdateTitle ()
        {
            if (!string.IsNullOrWhiteSpace (((Page)this.Element).Title)) {
                this.Title = ((Page)this.Element).Title;
            }
        }

        public SizeRequest GetDesiredSize (double widthConstraint, double heightConstraint)
        {
            return this.NativeView.GetSizeRequest (widthConstraint, heightConstraint, -1, -1);
        }

        public void SetElementSize (Xamarin.Forms.Size size)
        {
            this.Element.Layout (new Xamarin.Forms.Rectangle (this.Element.X, this.Element.Y, size.Width, size.Height));
        }

        public VisualElement Element {
            get;
            private set;
        }

        public UIView NativeView {
            get {
                return this.View;
            }
        }

        public UIViewController ViewController {
            get {
                return this;
            }
        }

        MainContainerPage ContainerPage { get { return Element as MainContainerPage; } }
        List<UIViewController> _pages;

        public SideMenuRenderer()
        {

        }

        void SetSelectedItem(int i)
        {
            ContainerPage.SelectedItem = ContainerPage.Children[i];
            SetContentViewController(_pages[i], true);
            HideMenuViewController();
        }
    
        public override void ViewWillDisappear (bool animated)
        {
            base.ViewWillDisappear (animated);
            if (this.View.Window != null) {
                this.View.Window.EndEditing (true);
            }
        }

        bool IsIphone5 {
            get {
                return UIScreen.MainScreen.ApplicationFrame.Height == 548;
            }
        }

        bool IsPhone {
            get {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone;
            }
        }

        class LeftMenuController : UIViewController
        {
            float _fontSize;
            float _rowHeight; 

            SideMenuRenderer _sideMenuRenderer;
            public LeftMenuController (SideMenuRenderer sideMenuRenderer) 
            {
                _sideMenuRenderer = sideMenuRenderer;
                _fontSize = 21;
                _rowHeight = 54;
            }

            public override void ViewDidLoad ()
            {
                base.ViewDidLoad ();

                var count = _sideMenuRenderer.ContainerPage.Children.Count;
                var tableView = new UITableView (new RectangleF (0, (View.Frame.Height - _rowHeight * count) / 2.0f, View.Frame.Width, _rowHeight * count), UITableViewStyle.Plain);
                tableView.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleWidth;
                tableView.Source = new TableSource (this);
                tableView.Opaque = false;
                tableView.BackgroundColor = UIColor.Clear;
                tableView.BackgroundView = null;
                tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
                tableView.Bounces = false;

                View.AddSubview (tableView);
            }

            class TableSource : UITableViewSource
            {
                LeftMenuController _menu;

                public TableSource (LeftMenuController menu) 
                {
                    _menu = menu;
                }

                public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
                {
                    return _menu._rowHeight;
                }

                public override int NumberOfSections (UITableView tableView)
                {
                    return 1;
                }

                public override int RowsInSection (UITableView tableview, int section)
                {
                    return _menu._sideMenuRenderer.ContainerPage.Children.Count;
                }

                static string cellIdentifier = "Cell";

                public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
                {
                    UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);

                    if (cell == null) {
                        cell = new UITableViewCell (UITableViewCellStyle.Default, cellIdentifier);
                        cell.BackgroundColor = UIColor.Clear;

                        cell.TextLabel.Font = UIFont.FromName("HelveticaNeue", _menu._fontSize);

                        cell.TextLabel.TextColor = UIColor.White;
                        cell.TextLabel.HighlightedTextColor = UIColor.LightGray;
                        cell.SelectedBackgroundView = new UIView();
                    }

                    cell.TextLabel.Text = _menu._sideMenuRenderer.ContainerPage.Children[indexPath.Row].Title;
                    cell.ImageView.Image = UIImage.FromBundle(_menu._sideMenuRenderer.ContainerPage.Children[indexPath.Row].Icon.File.Replace(".png", "_w.png"));

                    return cell;
                }

                public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
                {
                    tableView.DeselectRow (indexPath, true);

                    _menu._sideMenuRenderer.SetSelectedItem(indexPath.Row);
                }
                   
            }

        }
    }

}

