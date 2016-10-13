using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AutoClash.Pages
{
    /// <summary>
    /// The main page of the application
    /// </summary>
    public sealed partial class Main : Page
    {
        public Main(Frame frame)
        {
            this.InitializeComponent();
            this.mainSplitView.Content = frame;

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                //Debug.WriteLine("BackRequested");
                if (frame.CanGoBack)
                {
                    frame.GoBack();
                    a.Handled = true;

                    if (!frame.CanGoBack)
                    {
                        Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                }
            };
        }

        private void OnMenuButtonClicked(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
            ((RadioButton)sender).IsChecked = false;
        }

        private void OnHomeButtonChecked(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = false;
            if (mainSplitView.Content != null)
                ((Frame)mainSplitView.Content).Navigate(typeof(Pages.Home));

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void OnSettingsButtonChecked(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = false;
            if (mainSplitView.Content != null)
                ((Frame)mainSplitView.Content).Navigate(typeof(Settings));

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void OnAboutButtonChecked(object sender, RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = false;
            if (mainSplitView.Content != null)
                ((Frame)mainSplitView.Content).Navigate(typeof(About));

            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }
}
