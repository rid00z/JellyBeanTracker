//
// MRSideMenu
// 
// A monotouch version of the beautiful RESideMenu by Roman Efimov (https://github.com/romaonthego)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using System;
using MonoTouch.UIKit;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;

namespace MR
{
    public class MRSideMenu : UIViewController
    {
        public bool RightMenuEnabled { get; set; }

        UIView _menuViewContainer = new UIView();
        UIView _contentViewContainer = new UIView ();

        float _animationDuration = 0.35f;
        bool _interactivePopGestureRecognizerEnabled = true;

        CGAffineTransform _menuViewControllerTransformation = CGAffineTransform.MakeScale(1.5f, 1.5f);

        bool _scaleContentView = true;
        bool _scaleBackgroundImageView = true;
        bool _scaleMenuView = true;
        bool Visible;

        bool _parallaxEnabled = true;
        float _parallaxMenuMinimumRelativeValue = -15;
        float _parallaxMenuMaximumRelativeValue = 15;
        float _parallaxContentMinimumRelativeValue = -25;
        float _parallaxContentMaximumRelativeValue = 25;

        //      sideMenuViewController.BackgroundImage = UIImage.FromBundle ("Stars.png");
        //      sideMenuViewController.MenuPreferredStatusBarStyle = 1; // UIStatusBarStyleLightContent
        //      //sideMenuViewController.delegate = self;
        //      sideMenuViewController.ContentViewShadowColor = UIColor.Blue;
        //      sideMenuViewController.ContentViewShadowOffset = CGSizeMake(0, 0);
        //      sideMenuViewController.ContentViewShadowOpacity = 0.6;
        //      sideMenuViewController.ContentViewShadowRadius = 12;
        //      sideMenuViewController.ContentViewShadowEnabled = YES;

        bool _didNotifyDelegate;
        bool _ios7 = true;
        bool _bouncesHorizontally = true;
        bool _leftMenuVisible;
        bool _rightMenuVisible;
        PointF _originalPoint;

        bool _panGestureEnabled = true;
        bool _panFromEdge = true;
        float _panMinimumOpenThreshold = 60.0F;

        bool _contentViewShadowEnabled = false;
        UIColor _contentViewShadowColor = UIColor.Black;
        SizeF _contentViewShadowOffset = SizeF.Empty;
        float _contentViewShadowOpacity = 0.4f;
        float _contentViewShadowRadius = 8.0f;
        float _contentViewInLandscapeOffsetCenterX = 30F;
        float _contentViewInPortraitOffsetCenterX  = 30F;
        float _contentViewScaleValue = 0.7f;
        UIImageView _backgroundImageView;
        public UIImage BackgroundImage { get; set; }
        UIButton _contentButton;

        UIViewController _contentViewController, _leftMenuViewController, _rightMenuViewController;

        public MRSideMenu()
        {

        }

        public MRSideMenu(UIViewController contentViewController, UIViewController leftMenuViewController, UIViewController rightMenuViewController) 
        {
            _contentViewController = contentViewController;
            _leftMenuViewController = leftMenuViewController;
            _rightMenuViewController = rightMenuViewController;
        }

        public void SetControllers(UIViewController contentViewController, UIViewController leftMenuViewController, UIViewController rightMenuViewController) 
        {
            _contentViewController = contentViewController;
            _leftMenuViewController = leftMenuViewController;
            _rightMenuViewController = rightMenuViewController;
        } 

        public void Toggle()
        {
            if (Visible)
                HideMenuViewController();
            else
                PresentLeftMenuViewController();
        }

        public void PresentLeftMenuViewController()
        {
            PresentMenuViewContainer(_leftMenuViewController);
            ShowLeftMenuViewController();
        }

        public void PresentRightMenuViewController()
        {
            PresentMenuViewContainer(_rightMenuViewController);
            ShowRightMenuViewController ();
        }

        public void HideMenuViewController()
        {
            HideMenuViewControllerAnimated(true);
        }

        public void HideMenuViewController(object sender, EventArgs e)
        {
            HideMenuViewControllerAnimated(true);
        }

        public void SetContentViewController(UIViewController contentViewController, bool animated)
        {
            if (_contentViewController == contentViewController)
            {
                return;
            }

            if (!animated) {
                SetContentViewController(contentViewController);
            } else {
                AddChildViewController(contentViewController);
                contentViewController.View.Alpha = 0;
                contentViewController.View.Frame = _contentViewContainer.Bounds;
                _contentViewContainer.AddSubview(contentViewController.View);
                UIView.Animate(_animationDuration, () => {
                    contentViewController.View.Alpha = 1;
                }, () => {
                    HideViewController(_contentViewController);
                    contentViewController.DidMoveToParentViewController(this);
                    _contentViewController = contentViewController;

                    StatusBarNeedsAppearanceUpdate();
                    UpdateContentViewShadow();

                    if (Visible) {
                        AddContentViewControllerMotionEffects();
                    }
                });
            }
        }

        public override void ViewDidLoad()
        {
            View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _backgroundImageView = new UIImageView (View.Bounds) {
                Image = BackgroundImage,
                ContentMode = UIViewContentMode.ScaleAspectFill,
                AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight

            };
            _contentButton = new UIButton (RectangleF.Empty);
            _contentButton.TouchUpInside += HideMenuViewController;

            Add (_backgroundImageView);
            Add (_menuViewContainer);
            Add (_contentViewContainer);

            _menuViewContainer.Frame = View.Bounds;
            _menuViewContainer.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            if (_leftMenuViewController != null) {
                AddChildViewController (_leftMenuViewController);
                _leftMenuViewController.View.Frame = View.Bounds;
                _leftMenuViewController.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                _menuViewContainer.AddSubview(_leftMenuViewController.View);
                _leftMenuViewController.DidMoveToParentViewController (this);
            }

            if (_rightMenuViewController != null) {
                AddChildViewController(_rightMenuViewController);
                _rightMenuViewController.View.Frame = View.Bounds;
                _rightMenuViewController.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                _menuViewContainer.AddSubview (_rightMenuViewController.View);
                _rightMenuViewController.DidMoveToParentViewController(this);
            }

            _contentViewContainer.Frame = View.Bounds;
            _contentViewContainer.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            AddChildViewController (_contentViewController);
            _contentViewController.View.Frame = View.Bounds;
            _contentViewContainer.AddSubview(_contentViewController.View);
            _contentViewController.DidMoveToParentViewController (this);

            _menuViewContainer.Alpha = 0;
            if (_scaleBackgroundImageView)
                _backgroundImageView.Transform = CGAffineTransform.MakeScale(1.7f, 1.7f);

            AddMenuViewControllerMotionEffects ();

            if (_panGestureEnabled) {
                View.MultipleTouchEnabled = false;
                UIPanGestureRecognizer panGestureRecognizer = new UIPanGestureRecognizer(PanGestureRecognized);
                panGestureRecognizer.ShouldReceiveTouch += this.GestureRecognizerShouldReceiveTouch;
                View.AddGestureRecognizer (panGestureRecognizer);
            }

            UpdateContentViewShadow ();
        }

        void PresentMenuViewContainer(UIViewController menuViewController)
        {
            _menuViewContainer.Transform = CGAffineTransform.MakeIdentity();
            if (_scaleBackgroundImageView) {
                _backgroundImageView.Transform = CGAffineTransform.MakeIdentity();
                _backgroundImageView.Frame = View.Bounds;
            }
            _menuViewContainer.Frame = View.Bounds;
            if (_scaleMenuView) {
                _menuViewContainer.Transform = _menuViewControllerTransformation;
            }
            _menuViewContainer.Alpha = 0;
            if (_scaleBackgroundImageView)
                _backgroundImageView.Transform = CGAffineTransform.MakeScale(1.7f, 1.7f);

            //            if ([self.delegate conformsToProtocol:@protocol(RESideMenuDelegate)] && [self.delegate respondsToSelector:@selector(sideMenu:willShowMenuViewController:)]) {
            //                [self.delegate sideMenu:self willShowMenuViewController:menuViewController];
            //            }
        }

        void ShowLeftMenuViewController()
        {
            if (_leftMenuViewController == null) {
                return;
            }
            if (_rightMenuViewController != null)
                _rightMenuViewController.View.Hidden = true;
            View.Window.EndEditing (true);
            AddContentButton ();
            UpdateContentViewShadow ();

            UIView.Animate(_animationDuration, () => {
                if (_scaleContentView) {
                    _contentViewContainer.Transform = CGAffineTransform.MakeScale(_contentViewScaleValue, _contentViewScaleValue);
                } else {
                    _contentViewContainer.Transform = CGAffineTransform.MakeIdentity();
                }
                var landscape = UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft 
                    || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight;

                float pointAddX = View.Frame.Width;
                if (landscape)
                    pointAddX = View.Frame.Height;

                _contentViewContainer.Center = new PointF(_contentViewInLandscapeOffsetCenterX + pointAddX, _contentViewContainer.Center.Y);

                _menuViewContainer.Alpha = 1.0f;
                _menuViewContainer.Transform = CGAffineTransform.MakeIdentity();
                if (_scaleBackgroundImageView)
                    _backgroundImageView.Transform = CGAffineTransform.MakeIdentity();

            },() => {
                AddContentViewControllerMotionEffects();

                //                if (!Visible && [self.delegate conformsToProtocol:@protocol(RESideMenuDelegate)] && [self.delegate respondsToSelector:@selector(sideMenu:didShowMenuViewController:)]) {
                //                    [self.delegate sideMenu:self didShowMenuViewController:self.leftMenuViewController];
                //                }

                Visible = true;
                _leftMenuVisible = true;
            });

            StatusBarNeedsAppearanceUpdate ();
        }

        void ShowRightMenuViewController()
        {
            if (_rightMenuViewController == null) {
                return;
            }
            if (_leftMenuViewController != null)
                _leftMenuViewController.View.Hidden = true;
            _rightMenuViewController.View.Hidden = false;
            View.Window.EndEditing (true);
            AddContentButton();
            UpdateContentViewShadow();

            UIApplication.SharedApplication.BeginIgnoringInteractionEvents ();

            UIView.Animate(_animationDuration, () => {
                if (_scaleContentView) {
                    _contentViewContainer.Transform = 
                        CGAffineTransform.MakeScale(_contentViewScaleValue, _contentViewScaleValue);
                } else {
                    _contentViewContainer.Transform = CGAffineTransform.MakeIdentity();
                }

                bool landscape = UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeLeft
                    || UIApplication.SharedApplication.StatusBarOrientation == UIInterfaceOrientation.LandscapeRight;

                _contentViewContainer.Center 
                = new PointF(landscape ? -_contentViewInLandscapeOffsetCenterX : -_contentViewInPortraitOffsetCenterX, 
                    _contentViewContainer.Center.Y);

                _menuViewContainer.Alpha = 1.0f;
                _menuViewContainer.Transform = CGAffineTransform.MakeIdentity();
                if (_scaleBackgroundImageView)
                    _backgroundImageView.Transform = CGAffineTransform.MakeIdentity();

            }, () => {
                //                if (!_rightMenuVisible && [self.delegate conformsToProtocol:@protocol(RESideMenuDelegate)] && [self.delegate respondsToSelector:@selector(sideMenu:didShowMenuViewController:)]) {
                //                    [self.delegate sideMenu:self didShowMenuViewController:self.rightMenuViewController];
                //                }

                Visible = !(_contentViewContainer.Frame.Size.Width == View.Bounds.Size.Width 
                    && _contentViewContainer.Frame.Size.Height == View.Bounds.Size.Height 
                    && _contentViewContainer.Frame.Location.X == 0 
                    && _contentViewContainer.Frame.Location.Y == 0);
                _rightMenuVisible = Visible;
                UIApplication.SharedApplication.EndIgnoringInteractionEvents();
                AddContentViewControllerMotionEffects();
            });

            StatusBarNeedsAppearanceUpdate ();
        }

        void HideViewController(UIViewController viewController)
        {
            viewController.WillMoveToParentViewController(this);
            viewController.View.RemoveFromSuperview ();
            viewController.RemoveFromParentViewController ();
        }

        void HideMenuViewControllerAnimated(bool animated)
        {
            bool rightMenuVisible = _rightMenuVisible;
            //            if ([self.delegate conformsToProtocol:@protocol(RESideMenuDelegate)] && [self.delegate respondsToSelector:@selector(sideMenu:willHideMenuViewController:)]) {
            //                [self.delegate sideMenu:self willHideMenuViewController:rightMenuVisible ? self.rightMenuViewController : self.leftMenuViewController];
            //            }

            Visible = false;
            _leftMenuVisible = false;
            _rightMenuVisible = false;
            _contentButton.RemoveFromSuperview ();

            NSAction animationBlock = () => {

                _contentViewContainer.Transform = CGAffineTransform.MakeIdentity();
                _contentViewContainer.Frame = View.Bounds;
                if (_scaleMenuView) {
                    _menuViewContainer.Transform = _menuViewControllerTransformation;
                }
                _menuViewContainer.Alpha = 0;
                if (_scaleBackgroundImageView) {
                    _backgroundImageView.Transform = CGAffineTransform.MakeScale(1.7f, 1.7f);
                }
                if (_parallaxEnabled) {
                    if (_ios7) {
                        foreach (UIMotionEffect effect in _contentViewContainer.MotionEffects) {
                            _contentViewContainer.RemoveMotionEffect(effect);
                        }

                    }
                }
            };
            NSAction completionBlock = () => {
                //                if (!Visible && [strongSelf.delegate conformsToProtocol:@protocol(RESideMenuDelegate)] && [strongSelf.delegate respondsToSelector:@selector(sideMenu:didHideMenuViewController:)]) {
                //                    [strongSelf.delegate sideMenu:strongSelf didHideMenuViewController:rightMenuVisible ? strongSelf.rightMenuViewController : strongSelf.leftMenuViewController];
                //                }
            };

            if (animated) {
                //UIApplication.SharedApplication.BeginIgnoringInteractionEvents ();
                UIView.Animate (_animationDuration, animationBlock, completionBlock);
            } else {
                //animationBlock();
                //completionBlock();
            }
            StatusBarNeedsAppearanceUpdate ();
        }

        void AddContentButton()
        {
            //            if (_contentButton.Superview != null)
            //                return;
            //
            //            _contentButton.AutoresizingMask = UIViewAutoresizing.None;
            //            _contentButton.Frame = _contentViewContainer.Bounds;
            //            _contentButton.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            //            _contentViewContainer.AddSubview (_contentButton);
        }

        void StatusBarNeedsAppearanceUpdate()
        {
            UIView.Animate(0.3f, () => this.SetNeedsStatusBarAppearanceUpdate());
        }

        void UpdateContentViewShadow()
        {
            if (_contentViewShadowEnabled) {
                CALayer layer = _contentViewContainer.Layer;
                UIBezierPath path = UIBezierPath.FromRect(layer.Bounds);
                layer.ShadowPath = path.CGPath;
                layer.ShadowColor = _contentViewShadowColor.CGColor;
                layer.ShadowOffset = _contentViewShadowOffset;
                layer.ShadowOpacity = _contentViewShadowOpacity;
                layer.ShadowRadius = _contentViewShadowRadius;
            }
        }

        void AddMenuViewControllerMotionEffects()
        {
            if (_parallaxEnabled) {
                if (_ios7) {
                    if (_menuViewContainer.MotionEffects != null)
                        foreach (UIMotionEffect effect in _menuViewContainer.MotionEffects) {
                            _menuViewContainer.RemoveMotionEffect(effect);
                        }

                    UIInterpolatingMotionEffect interpolationHorizontal = 
                        new UIInterpolatingMotionEffect ("center.x", UIInterpolatingMotionEffectType.TiltAlongHorizontalAxis);

                    interpolationHorizontal.MinimumRelativeValue = NSObject.FromObject(_parallaxMenuMinimumRelativeValue);
                    interpolationHorizontal.MaximumRelativeValue = NSObject.FromObject(_parallaxMenuMaximumRelativeValue);

                    UIInterpolatingMotionEffect interpolationVertical = 
                        new UIInterpolatingMotionEffect("center.y", UIInterpolatingMotionEffectType.TiltAlongVerticalAxis);
                    interpolationVertical.MinimumRelativeValue = NSObject.FromObject(_parallaxMenuMinimumRelativeValue);
                    interpolationVertical.MaximumRelativeValue = NSObject.FromObject(_parallaxMenuMaximumRelativeValue);

                    _menuViewContainer.AddMotionEffect(interpolationHorizontal);
                    _menuViewContainer.AddMotionEffect(interpolationVertical);
                };
            }
        }

        void AddContentViewControllerMotionEffects()
        {
            if (_parallaxEnabled) {
                if (_ios7) {
                    if (_contentViewContainer.MotionEffects != null)
                        foreach (UIMotionEffect effect in _contentViewContainer.MotionEffects) {
                            _contentViewContainer.RemoveMotionEffect(effect);
                        }

                    UIView.Animate(0.2,() => {
                        UIInterpolatingMotionEffect interpolationHorizontal =
                            new UIInterpolatingMotionEffect ("center.x", UIInterpolatingMotionEffectType.TiltAlongHorizontalAxis);

                        interpolationHorizontal.MinimumRelativeValue = NSObject.FromObject(_parallaxContentMinimumRelativeValue);
                        interpolationHorizontal.MaximumRelativeValue = NSObject.FromObject(_parallaxContentMaximumRelativeValue);

                        UIInterpolatingMotionEffect interpolationVertical =
                            new UIInterpolatingMotionEffect ("center.y", UIInterpolatingMotionEffectType.TiltAlongVerticalAxis);
                        interpolationVertical.MinimumRelativeValue = NSObject.FromObject(_parallaxContentMinimumRelativeValue);
                        interpolationVertical.MaximumRelativeValue = NSObject.FromObject(_parallaxContentMaximumRelativeValue);

                        _contentViewContainer.AddMotionEffect(interpolationHorizontal);
                        _contentViewContainer.AddMotionEffect(interpolationVertical);
                    });
                }
            }
        }

        bool GestureRecognizerShouldReceiveTouch (UIGestureRecognizer gestureRecognizer, UITouch touch)
        {
            if (_ios7) {
                if (_interactivePopGestureRecognizerEnabled && (_contentViewController is UINavigationController)) {
                    UINavigationController navigationController = _contentViewController as UINavigationController;
                    if (navigationController.ViewControllers.Length > 1 && navigationController.InteractivePopGestureRecognizer.Enabled) {
                        return false;
                    }
                }
            }

            if (_panFromEdge && gestureRecognizer is UIPanGestureRecognizer && !Visible) {
                var point = touch.LocationInView(gestureRecognizer.View);
                if (point.X < 20.0 || point.X > View.Frame.Size.Width - 20.0) {
                    return true;
                } else {
                    return false;
                }
            }

            return true;
        }

        void PanGestureRecognized(UIPanGestureRecognizer recognizer)
        {
            //          if ([self.delegate conformsToProtocol:@protocol(RESideMenuDelegate)] && [self.delegate respondsToSelector:@selector(sideMenu:didRecognizePanGesture:)])
            //              [self.delegate sideMenu:self didRecognizePanGesture:recognizer];

            if (!_panGestureEnabled) {
                return;
            }

            PointF point = recognizer.TranslationInView(View);

            if (recognizer.State == UIGestureRecognizerState.Began) {
                UpdateContentViewShadow();

                _originalPoint = new PointF((float)(_contentViewContainer.Center.X - _contentViewContainer.Bounds.Width / 2.0),
                    (float)(_contentViewContainer.Center.Y - _contentViewContainer.Bounds.Height / 2.0));
                _menuViewContainer.Transform = CGAffineTransform.MakeIdentity();
                if (_scaleBackgroundImageView) {
                    _backgroundImageView.Transform = CGAffineTransform.MakeIdentity();
                    _backgroundImageView.Frame = View.Bounds;
                }
                _menuViewContainer.Frame = View.Bounds;
                AddContentButton();
                View.Window.EndEditing(true);
                _didNotifyDelegate = false;
            }

            if (recognizer.State == UIGestureRecognizerState.Changed) {
                float delta = 0;
                if (Visible) {
                    delta = _originalPoint.X != 0 ? (point.X + _originalPoint.X) / _originalPoint.X : 0;
                } else {
                    delta = point.X / View.Frame.Size.Width;
                }
                delta = Math.Min(Math.Abs(delta), 1.6F);

                float contentViewScale = _scaleContentView ? 1 - ((1 - _contentViewScaleValue) * delta) : 1;

                float backgroundViewScale = 1.7f - (0.7f * delta);
                float menuViewScale = 1.5f - (0.5f * delta);

                if (!_bouncesHorizontally) {
                    contentViewScale = Math.Max(contentViewScale, _contentViewScaleValue);
                    backgroundViewScale = Math.Max(backgroundViewScale, 1.0F);
                    menuViewScale = Math.Max(menuViewScale, 1.0F);
                }

                _menuViewContainer.Alpha = delta;

                if (_scaleBackgroundImageView) {
                    _backgroundImageView.Transform = CGAffineTransform.MakeScale(backgroundViewScale, backgroundViewScale);
                }

                if (_scaleMenuView) {
                    _menuViewContainer.Transform = CGAffineTransform.MakeScale(menuViewScale, menuViewScale);
                }

                if (_scaleBackgroundImageView) {
                    if (backgroundViewScale < 1) {
                        _backgroundImageView.Transform = CGAffineTransform.MakeIdentity();
                    }
                }

                if (!_bouncesHorizontally && Visible) {
                    if (_contentViewContainer.Frame.Location.X > _contentViewContainer.Frame.Size.Width / 2.0)
                        point.X = Math.Min(0.0F, point.X);

                    if (_contentViewContainer.Frame.Location.X < -(_contentViewContainer.Frame.Size.Width / 2.0))
                        point.X = Math.Max(0.0F, point.X);
                }

                // Limit size
                //
                if (point.X < 0) {
                    point.X = Math.Max(point.X, - UIScreen.MainScreen.Bounds.Size.Height);
                } else {
                    point.X = Math.Min(point.X, UIScreen.MainScreen.Bounds.Size.Height);
                }
                recognizer.SetTranslation (point, View);

                if (!_didNotifyDelegate) {
                    if (point.X > 0) {
                        //                      if (!Visible && [self.delegate conformsToProtocol:@protocol(RESideMenuDelegate)] && [self.delegate respondsToSelector:@selector(sideMenu:willShowMenuViewController:)]) {
                        //                          [self.delegate sideMenu:self willShowMenuViewController:self.leftMenuViewController];
                        //                      }
                    }
                    if (point.X < 0) {

                        //                      if (!self.visible && [self.delegate conformsToProtocol:@protocol(RESideMenuDelegate)] && [self.delegate respondsToSelector:@selector(sideMenu:willShowMenuViewController:)]) {
                        //                          //[self.delegate sideMenu:self willShowMenuViewController:self.rightMenuViewController];
                        //                      }
                    }
                    _didNotifyDelegate = true;
                }

                if (contentViewScale > 1) {
                    float oppositeScale = (1 - (contentViewScale - 1));
                    _contentViewContainer.Transform = CGAffineTransform.MakeScale(oppositeScale, oppositeScale);
                    _contentViewContainer.Transform = CGAffineTransform.Translate(_contentViewContainer.Transform, point.X, 0);
                } else {
                    _contentViewContainer.Transform = CGAffineTransform.MakeScale(contentViewScale, contentViewScale);
                    _contentViewContainer.Transform = CGAffineTransform.Translate(_contentViewContainer.Transform, point.X, 0);
                }

                if (_leftMenuViewController != null)
                    _leftMenuViewController.View.Hidden = _contentViewContainer.Frame.Location.X < 0;
                if (_rightMenuViewController != null)
                    _rightMenuViewController.View.Hidden = _contentViewContainer.Frame.Location.X > 0;

                if (_leftMenuViewController == null && _contentViewContainer.Frame.Location.X > 0) {
                    _contentViewContainer.Transform = CGAffineTransform.MakeIdentity();
                    _contentViewContainer.Frame = View.Bounds;
                    Visible = false;
                    _leftMenuVisible = false;
                } else  if (_rightMenuViewController == null && _contentViewContainer.Frame.Location.X < 0) {
                    _contentViewContainer.Transform = CGAffineTransform.MakeIdentity();
                    _contentViewContainer.Frame = View.Bounds;
                    Visible = false;
                    _rightMenuVisible = false;
                }

                StatusBarNeedsAppearanceUpdate ();
            }

            if (recognizer.State == UIGestureRecognizerState.Ended) {
                _didNotifyDelegate = false;

                if (_leftMenuViewController != null)
                    _leftMenuViewController.View.Hidden = false;
                if (_rightMenuViewController != null)
                    _rightMenuViewController.View.Hidden = false;

                if (_panMinimumOpenThreshold > 0 && (
                    (_contentViewContainer.Frame.Location.X < 0 && _contentViewContainer.Frame.Location.X > -((int)_panMinimumOpenThreshold)) ||
                    (_contentViewContainer.Frame.Location.X > 0 && _contentViewContainer.Frame.Location.X < _panMinimumOpenThreshold))
                ) {
                    HideMenuViewController (this, null);
                }
                else if (_contentViewContainer.Frame.Location.X == 0) {
                    HideMenuViewControllerAnimated(false);
                }
                else {
                    if (recognizer.VelocityInView(View).X > 0) {
                        if (_contentViewContainer.Frame.Location.X < 0) {
                            HideMenuViewController (this, null);
                        } else {
                            if (_leftMenuViewController != null) {
                                ShowLeftMenuViewController ();
                            }
                        }
                    } else {
                        if (_contentViewContainer.Frame.Location.X < 20) {
                            if (_rightMenuViewController != null) {
                                ShowRightMenuViewController ();
                            }
                        } else {
                            HideMenuViewController (this, null);
                        }
                    }
                }
            }
        }

        void SetBackgroundImage(UIImage backgroundImage)
        {
            BackgroundImage = backgroundImage;
            if (_backgroundImageView != null)
                _backgroundImageView.Image = backgroundImage;
        }

        void SetContentViewController(UIViewController contentViewController)
        {
            if (_contentViewController == null) {
                _contentViewController = contentViewController;
                return;
            }
            HideViewController(_contentViewController);
            _contentViewController = contentViewController;

            AddChildViewController(_contentViewController);
            _contentViewController.View.Frame = View.Bounds;
            _contentViewContainer.AddSubview (_contentViewController.View);
            _contentViewController.DidMoveToParentViewController (this);

            UpdateContentViewShadow ();

            if (Visible) {
                AddContentViewControllerMotionEffects ();
            }
        }

        void SetLeftMenuViewController(UIViewController leftMenuViewController)
        {
            if (_leftMenuViewController == null) {
                _leftMenuViewController = leftMenuViewController;
                return;
            }
            HideViewController(_leftMenuViewController);
            _leftMenuViewController = leftMenuViewController;

            AddChildViewController(_leftMenuViewController);
            _leftMenuViewController.View.Frame = View.Bounds;
            _leftMenuViewController.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _menuViewContainer.AddSubview(_leftMenuViewController.View);
            _leftMenuViewController.DidMoveToParentViewController (this);

            AddMenuViewControllerMotionEffects();
            View.BringSubviewToFront (_contentViewContainer);
        }

        void SetRightMenuViewController(UIViewController rightMenuViewController)
        {
            if (_rightMenuViewController == null) {
                _rightMenuViewController = rightMenuViewController;
                return;
            }
            HideViewController(_rightMenuViewController);
            _rightMenuViewController = rightMenuViewController;

            AddChildViewController(_rightMenuViewController);
            _rightMenuViewController.View.Frame = View.Bounds;
            _rightMenuViewController.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            _menuViewContainer.AddSubview(_rightMenuViewController.View);
            _rightMenuViewController.DidMoveToParentViewController(this);

            AddMenuViewControllerMotionEffects ();
            View.BringSubviewToFront(_contentViewContainer);
        }

        public override bool ShouldAutorotate ()
        {
            return _contentViewController.ShouldAutorotate();
        }

        public override void WillAnimateRotation (UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            if (Visible) {
                _menuViewContainer.Bounds = View.Bounds;
                _contentViewContainer.Transform = CGAffineTransform.MakeIdentity();
                _contentViewContainer.Frame = View.Bounds;

                if (_scaleContentView) {
                    _contentViewContainer.Transform = CGAffineTransform.MakeScale(_contentViewScaleValue, _contentViewScaleValue);
                } else {
                    _contentViewContainer.Transform = CGAffineTransform.MakeIdentity();
                }

                bool isLandscape = UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeLeft
                    || UIDevice.CurrentDevice.Orientation == UIDeviceOrientation.LandscapeRight;

                PointF center;
                if (_leftMenuVisible) {
                    center = new PointF(
                        isLandscape ? _contentViewInLandscapeOffsetCenterX + View.Frame.Height : _contentViewInPortraitOffsetCenterX + View.Frame.Width
                        , _contentViewContainer.Center.Y);
                } else {
                    center = new PointF(
                        isLandscape ? -_contentViewInLandscapeOffsetCenterX : -_contentViewInPortraitOffsetCenterX
                        , _contentViewContainer.Center.Y);
                }

                _contentViewContainer.Center = center;
            }

            UpdateContentViewShadow ();
        }

        public override UIStatusBarStyle PreferredStatusBarStyle ()
        {
            UIStatusBarStyle statusBarStyle = UIStatusBarStyle.Default;
            if (_ios7) {
                statusBarStyle = Visible ? base.PreferredStatusBarStyle() : _contentViewController.PreferredStatusBarStyle();
                if (_contentViewContainer.Frame.Location.Y > 10) {
                    statusBarStyle = base.PreferredStatusBarStyle();
                } else {
                    statusBarStyle = _contentViewController.PreferredStatusBarStyle();
                }
            }
            return statusBarStyle;
        }

        public override bool PrefersStatusBarHidden ()
        {
            bool statusBarHidden = false;
            if (_ios7) {
                statusBarHidden = Visible ? base.PrefersStatusBarHidden() : _contentViewController.PrefersStatusBarHidden();
                if (_contentViewContainer.Frame.Location.Y > 10) {
                    statusBarHidden = base.PrefersStatusBarHidden();
                } else {
                    statusBarHidden = _contentViewController.PrefersStatusBarHidden();
                }
            }
            return statusBarHidden;
        }

        public override UIStatusBarAnimation PreferredStatusBarUpdateAnimation {
            get {
                UIStatusBarAnimation statusBarAnimation = UIStatusBarAnimation.None;
                if (_ios7) {
                    statusBarAnimation = Visible ? _leftMenuViewController.PreferredStatusBarUpdateAnimation : _contentViewController.PreferredStatusBarUpdateAnimation;
                    if (_contentViewContainer.Frame.Location.Y > 10) {
                        statusBarAnimation = _leftMenuViewController.PreferredStatusBarUpdateAnimation;
                    } else {
                        statusBarAnimation = _contentViewController.PreferredStatusBarUpdateAnimation;
                    }
                }
                return statusBarAnimation;
            }
        }

    }

}

