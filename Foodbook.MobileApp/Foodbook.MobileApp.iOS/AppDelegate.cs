using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using CarouselView.FormsPlugin.iOS;
using FFImageLoading.Forms.Touch;
using FFImageLoading;
using PushNotification.Plugin;
using Foodbook.MobileApp.PushNotification;
using Foodbook.MobileApp.Tools;
using PNH = Plugin.NotificationHub;
using WindowsAzure.Messaging;

namespace Foodbook.MobileApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        private SBNotificationHub Hub { get; set; }
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            CarouselViewRenderer.Init();

            CachedImageRenderer.Init();
            ImageService.Instance.Initialize();
            FAB.iOS.FloatingActionButtonRenderer.InitControl();
            CrossPushNotification.Initialize<CrossPushNotificationListener>();
            CarouselViewRenderer.Init();

            LoadApplication(new App());
       
            return base.FinishedLaunching(app, options);
        }


        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {


            if (CrossPushNotification.Current is IPushNotificationHandler)
            {
                ((IPushNotificationHandler)CrossPushNotification.Current).OnErrorReceived(error);

            }


        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {

            if (!string.IsNullOrEmpty(LocalDataSecureStorage.GetToken()))
            {
                Hub = new SBNotificationHub(PushNotificationSettings.CONNECTION_STRING, PushNotificationSettings.HUB_NAME);

                // Unregister any previous instances using the device token
                Hub.UnregisterAllAsync(deviceToken, (error) =>
                {
                    if (error != null)
                    {
                    // Error unregistering
                    return;
                    }
                    string[] tags = new string[] { LocalDataSecureStorage.GetEmail() };
                // Register this device with the notification hub
                Hub.RegisterNativeAsync(deviceToken, new NSSet(tags), (registerError) =>
                {
                    if (registerError != null)
                    {
                        // Error registering
                    }
                });
            });
        }

        }

        public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
        {
            application.RegisterForRemoteNotifications();
        }

        //Uncomment if using remote background notifications.To support this background mode, enable the Remote notifications option from the Background modes section of iOS project properties. (You can also enable this support by including the UIBackgroundModes key with the remote-notification value in your app�s Info.plist file.)
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            if (CrossPushNotification.Current is IPushNotificationHandler)
            {
                ((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);

            }
        }


        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {

            if (CrossPushNotification.Current is IPushNotificationHandler)
            {
                ((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);

            }
        }
    }
}
