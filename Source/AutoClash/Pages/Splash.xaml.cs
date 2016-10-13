﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.ApplicationModel.Activation;
//using Splashscreenapp.Common;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/p/?LinkID=234238

namespace AutoClash.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    partial class Splash : Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;

        public Splash(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();

            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            splash = splashscreen;

            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();

                // Optional: Add a progress ring to your splash screen to show users that content is loading
                //PositionRing();
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();

            // Restore the saved session state if necessary
            //await RestoreStateAsync(loadState);
        }

        //async void RestoreStateAsync(bool loadState)
        //{
        //    if (loadState)
        //    {
        //        // TODO: write code to load state

        //    }
        //}
        internal void UpdateStatus(string status)
        {
            splashStatus.Text = status;
        }

        // Position the extended splash screen image in the same location as the system splash screen image.
        void PositionImage()
        {
            SplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            SplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            SplashImage.Height = splashImageRect.Height;
            SplashImage.Width = splashImageRect.Width;

            ProgressPanel.Margin = new Thickness { Top = (splashImageRect.Y + splashImageRect.Height + splashImageRect.Height * 0.1) };
            //ProgressPanel.SetValue(Canvas.LeftProperty, (splashImageRect.Width * 0.5) - (ProgressPanel.Width * 0.5));
            //ProgressPanel.SetValue(Canvas.TopProperty, (splashImageRect.Y + splashImageRect.Height + splashImageRect.Height * 0.1));


        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
                //PositionRing();
            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;

            // Complete app setup operations here...
        }

        void DismissExtendedSplash()
        {
            // Navigate to Home page.
            rootFrame.Navigate(typeof(Pages.Home));

            // Place the frame in main splitview content.
            Window.Current.Content = new Pages.Main(rootFrame);
        }

        void DismissSplashButton_Click(object sender, RoutedEventArgs e)
        {
            DismissExtendedSplash();
        }
    }
}
