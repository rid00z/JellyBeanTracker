using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;
using JellyBeanTracker.Mobile.Pages;

namespace JellyBeanTracker.Mobile.PageModels
{
    public enum ViewModelTabArea { JellyBeans, Graph, MyBean, Report }

    public abstract class BasePageModel : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BasePageModel PreviousViewModel { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged();
            }
        }

        //public abstract ViewModelTabArea TabArea { get; }

        public virtual void ReverseInit(object value) { }

        protected void PushViewModel<T> () where T : BasePageModel
        {
            PushViewModel<T> (null);
        }

        public static Page ResolveViewModel<T>(object data)
            where T : BasePageModel
        {
            var viewModel = TinyIoC.TinyIoCContainer.Current.Resolve<T>();

            return ResolveViewModel<T>(data, viewModel);
        }

        public static Page ResolveViewModel<T>(object data, BasePageModel viewModel)
            where T : BasePageModel
        {
            var name = typeof(T).Name.Replace ("Model", string.Empty);
            var pageType = Type.GetType ("JellyBeanTracker.Mobile.Pages." + name);
            var page = (Page)TinyIoC.TinyIoCContainer.Current.Resolve (pageType);

            page.BindingContext = viewModel;

            var initMethod = TinyIoC.TypeExtensions.GetMethod (typeof(T), "Init");
            if (initMethod != null) {
                if (initMethod.GetParameters ().Length > 0) 
                {
                    if (data == null)
                        data = new object();

                    initMethod.Invoke (viewModel, new object[] { data });
                }
                else 
                    initMethod.Invoke (viewModel, null);
            }

            var vmProperty = TinyIoC.TypeExtensions.GetProperty(pageType, "PageModel");
            if (vmProperty != null)
                vmProperty.SetValue (page, viewModel);

            var vmPageBindingContext = TinyIoC.TypeExtensions.GetProperty(pageType, "BindingContext");
            if (vmPageBindingContext != null)
                vmPageBindingContext.SetValue (page, viewModel);

            var initMethodPage = TinyIoC.TypeExtensions.GetMethod (pageType, "Init"); 
            if (initMethodPage != null)
                initMethodPage.Invoke (page, null);

            return page;
        }

        protected void PushViewModel<T> (object data, bool model = false) where T : BasePageModel
        {
            BasePageModel viewModel = TinyIoC.TinyIoCContainer.Current.Resolve<T>();;

            var page = ResolveViewModel<T> (data, viewModel);

            viewModel.PreviousViewModel = this;

            ITabbedNavigation tabbedNav = TinyIoC.TinyIoCContainer.Current.Resolve<ITabbedNavigation> ();

            if (!model)
            {
                tabbedNav.PushView (viewModel, page, model);
            }
            else
            {
                var navContainer = new BaseNav(page);
                tabbedNav.PushView(viewModel, navContainer, model);
            }

        }

        protected void PopViewModel(bool model = false)
        {
            ITabbedNavigation tabbedNav = TinyIoC.TinyIoCContainer.Current.Resolve<ITabbedNavigation> ();
            tabbedNav.PopView (model);
        }

        protected void PopViewModel(object data, bool model = false)
        {
            if (PreviousViewModel != null && data != null) {
                var initMethod = PreviousViewModel.GetType().GetRuntimeMethod ("ReverseInit", new Type[] { PreviousViewModel.GetType() });
                if (initMethod != null) {
                    initMethod.Invoke (PreviousViewModel, new object[] { data });
                }
            }
            ITabbedNavigation tabbedNav = TinyIoC.TinyIoCContainer.Current.Resolve<ITabbedNavigation> ();
            tabbedNav.PopView (model);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
