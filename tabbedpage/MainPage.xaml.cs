using Bertuzzi.MAUI.EventAggregator;
#if ANDROID
using Google.Android.Material.BottomNavigation;
using Android.Views;
#elif IOS
using UIKit;
#endif
namespace tabbedpage
{
    public partial class MainPage : TabbedPage
    {
        int TabBarHeight;
        public MainPage()
        {
            InitializeComponent();
            EventAggregator.Instance.RegisterHandler<bool>(ExecuteHiddenTab);
        }

        private void ExecuteHiddenTab(bool obj)
        {

#if ANDROID
            var root = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.FindViewById(Android.Resource.Id.Content).RootView;
            FindAndHideBottomNavigationView(root);
            void FindAndHideBottomNavigationView(Android.Views.View v)
            {
                if (v is ViewGroup)
                {
                    ViewGroup viewgroup = (ViewGroup)v;
                    for (int i = 0; i < viewgroup.ChildCount; i++)
                    {
                        Android.Views.View v1 = viewgroup.GetChildAt(i);
                        if (v1 is BottomNavigationView)
                        {
                            if (v1.LayoutParameters.Height > 0)
                                TabBarHeight = v1.LayoutParameters.Height;
                            v1.Visibility = obj ? ViewStates.Gone : ViewStates.Visible;
                            if (obj)
                                SetContentBottomMargin(0);
                            else
                                SetContentBottomMargin(TabBarHeight);
                            void SetContentBottomMargin(int bottomMargin)
                            {
                                var layoutContent = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity.FindViewById(Resource.Id.navigationlayout_content);
                                if (layoutContent != null && layoutContent.LayoutParameters is ViewGroup.MarginLayoutParams cl)
                                {
                                    cl.BottomMargin = bottomMargin;
                                }
                            }
                            break;
                        }
                        else
                            FindAndHideBottomNavigationView(v1);
                    }
                }

            }
#elif IOS
            var rootView = UIApplication.SharedApplication.KeyWindow.RootViewController;
#endif
        }
    }
}