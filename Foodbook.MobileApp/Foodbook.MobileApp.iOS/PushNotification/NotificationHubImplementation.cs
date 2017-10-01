using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Foodbook.MobileApp.PushNotification;
using Foodbook.MobileApp.Tools;
using WindowsAzure.Messaging;
using Foodbook.MobileApp.iOS.PushNotification;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationHubImplementation))]
namespace Foodbook.MobileApp.iOS.PushNotification
{
    public class NotificationHubImplementation : iOSPushNotificationInterface
    {
        private SBNotificationHub Hub { get; set; }

        public void Init()
        {
            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }       
    }
}