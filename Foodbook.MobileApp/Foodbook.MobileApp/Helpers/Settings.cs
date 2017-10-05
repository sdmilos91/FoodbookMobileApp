// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Foodbook.MobileApp.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string PushNotificationsSettingsKey = "ENABLE_PUSH_NOTIFICATIONS";
		private static readonly bool EnablePushNotifications = true;

		#endregion


		public static bool PushNotificationsSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(PushNotificationsSettingsKey, EnablePushNotifications);
			}
			set
			{
				AppSettings.AddOrUpdateValue(PushNotificationsSettingsKey, value);
			}
		}

	}
}