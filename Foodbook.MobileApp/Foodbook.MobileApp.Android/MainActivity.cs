
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using CarouselView.FormsPlugin.Android;
using FFImageLoading;
using FFImageLoading.Forms.Droid;
using Plugin.Permissions;
using Plugin.SecureStorage;
using PushNotification.Plugin;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Foodbook.MobileApp.Droid
{
    [Activity(Label = "Foodbook", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

            global::Xamarin.Forms.Forms.Init(this, bundle);

            DisplayCrashReport();

            CachedImageRenderer.Init();

            ImageService.Instance.Initialize();
            CarouselViewRenderer.Init();
            UserDialogs.Init(this);
            SecureStorageImplementation.StoragePassword = "FoodbookPass";
            CarouselViewRenderer.Init();
            LoadApplication(new App());
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private static void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        internal static void LogUnhandledException(Exception exception)
        {

            try
            {
                const string errorFileName = "Fatal.log";

                var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var errorFilePath = Path.Combine(libraryPath, errorFileName);
                var errorMessage = String.Format("Time: {0}\r\nError: Unhandled Exception\r\n{1}",
                DateTime.Now, exception.ToString());
                File.WriteAllText(errorFilePath, errorMessage);                
                

                // Log to Android Device Logging.
                Android.Util.Log.Error("Crash Report", errorMessage);
            }
            catch(Exception ex)
            {
                int i = 2;
                // just suppress any error logging exceptions
            }
        }

        private void DisplayCrashReport()
        {
            const string errorFilename = "Fatal.log";

            var libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var extPath = Android.OS.Environment.ExternalStorageDirectory;
            var errorFilePath = Path.Combine(libraryPath, errorFilename);
            var extFilePath = Path.Combine(extPath.AbsolutePath, errorFilename);

            if (!File.Exists(errorFilePath))
            {
                return;
            }

            var errorText = File.ReadAllText(errorFilePath);

            File.AppendAllText(extFilePath, errorText + "\r\n");


        }

    }
}

