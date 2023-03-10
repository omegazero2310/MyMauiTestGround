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
        const float CustomTabBarHeight = 60.0f;
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
#if IOS13_0_OR_GREATER
            var rootView = UIApplication.SharedApplication.KeyWindow.RootViewController;
            FindAndHideTabbar(rootView);
#else
            foreach (var scene in UIApplication.SharedApplication.ConnectedScenes)
            {
                if (scene is UIWindowScene windowScene)
                {
                    foreach (var windows in windowScene.Windows)
                    {
                        if (windows.IsKeyWindow)
                        {
                            FindAndHideTabbar(windows.RootViewController);
                            break;
                        }
                    }
                }
            }
#endif
            void FindAndHideTabbar(UIViewController views)
            {
                if (views is UITabBarController tabControllerview)
                {
                    tabControllerview.TabBar.Hidden = obj;
                }
                else
                {
                    foreach (var uiView in views.ChildViewControllers)
                    {
                        if (uiView is UITabBarController tabController)
                        {
                            tabController.TabBar.Hidden = obj;
                        }
                        else
                        {
                            if (uiView.ChildViewControllers?.Length > 0)
                            {
                                FindAndHideTabbar(uiView);
                            }
                        }
                    }
                }
                
            }
#endif
        }
    }
}