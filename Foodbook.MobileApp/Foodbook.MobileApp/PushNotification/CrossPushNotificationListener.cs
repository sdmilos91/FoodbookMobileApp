using Foodbook.MobileApp.Tools;
using Newtonsoft.Json.Linq;
using Plugin.NotificationHub;
using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Foodbook.MobileApp.PushNotification
{
    public class CrossPushNotificationListener : IPushNotificationListener
    {
        //Here you will receive all push notification messages
        //Messages arrives as a dictionary, the device type is also sent in order to check specific keys correctly depending on the platform.
        void IPushNotificationListener.OnMessage(JObject parameters, DeviceType deviceType)
        {
            Debug.WriteLine("Message Arrived");
        }
        //Gets the registration token after push registration
        void IPushNotificationListener.OnRegistered(string Token, DeviceType deviceType)
        {
            LocalDataSecureStorage.SaveNotificationToken(Token);
            Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", Token));

            if (!string.IsNullOrEmpty(LocalDataSecureStorage.GetToken()))
            {
                if (Device.OS != TargetPlatform.iOS)
                {
                    CrossNotificationHub.Current.Register(PushNotificationSettings.CONNECTION_STRING, PushNotificationSettings.HUB_NAME, Token, LocalDataSecureStorage.GetEmail());
                }
                else
                {
                    //Plugin za iOS nije dobro radio registraciju pa sam morao da je eksplicitno iniciram i da se registrujem na Azure Notification hub 
                    DependencyService.Get<iOSPushNotificationInterface>().Init();
                }
                Debug.WriteLine(string.Format("Push Notification - Device Registered - TAG : {0}", LocalDataSecureStorage.GetEmail()));
                
            }
        }
        //Fires when device is unregistered
        void IPushNotificationListener.OnUnregistered(DeviceType deviceType)
        {
            Debug.WriteLine("Push Notification - Device Unnregistered");
            CrossNotificationHub.Current.Unregister();
        }

        //Fires when error
        void IPushNotificationListener.OnError(string message, DeviceType deviceType)
        {
            Debug.WriteLine(string.Format("Push notification error - {0}", message));
        }

        //Enable/Disable Showing the notification
        bool IPushNotificationListener.ShouldShowNotification()
        {
            return true;
        }
    }
}
