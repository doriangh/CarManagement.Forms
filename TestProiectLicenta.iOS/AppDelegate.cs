﻿using FFImageLoading.Forms.Platform;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using PanCardView.iOS;
using Plugin.InputKit.Platforms.iOS;
using Refractored.XamForms.PullToRefresh.iOS;
using Rg.Plugins.Popup;
using RoundedBoxView.Forms.Plugin.iOSUnified;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace TestProiectLicenta.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Popup.Init();
            Config.Init();
            PullToRefreshLayoutRenderer.Init();
            CachedImageRenderer.Init();

            Forms.Init();
            CardsViewRenderer.Preserve();
            RoundedBoxViewRenderer.Init();
            ImageCircleRenderer.Init();
            Xamarin.FormsMaps.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}