using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	public class Settings
	{
		private const string SETTINGS_PATH = "AdvancedInputField/Settings";

		private static SettingsData data;

		private static SettingsData Data
		{
			get
			{
				if(data == null)
				{
					data = Resources.Load(SETTINGS_PATH) as SettingsData;
				}

				return data;
			}
		}

		public static MobileKeyboardBehaviour MobileKeyboardBehaviour { get { return Data.MobileKeyboardBehaviour; } }
		public static ActionBar MobileActionBarPrefab { get { return Data.MobileActionBarPrefab; } }
		public static Transform MobileSelectionCursorsPrefab { get { return Data.MobileSelectionCursorsPrefab; } }
		public static float MobileSelectionCursorsScale { get { return Data.MobileSelectionCursorsScale; } }
	}
}
