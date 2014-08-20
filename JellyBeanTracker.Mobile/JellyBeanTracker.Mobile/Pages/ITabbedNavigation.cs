using System;
using Xamarin.Forms;
using JellyBeanTracker.Mobile.PageModels;

namespace JellyBeanTracker.Mobile.Pages
{
	public interface ITabbedNavigation
	{
        void PushView(BasePageModel viewModelToPush, Page pageToPush, bool model);
        void PopView(bool model);
	}

}



