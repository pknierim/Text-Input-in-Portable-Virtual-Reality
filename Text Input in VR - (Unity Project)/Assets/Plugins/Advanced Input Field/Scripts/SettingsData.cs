using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	/// <summary>The behaviour to determine which keyboard type to use</summary>
	public enum MobileKeyboardBehaviour { USE_HARDWARE_KEYBOARD_WHEN_AVAILABLE, ALWAYS_USE_TOUCHSCREENKEYBOARD, ALWAYS_USE_HARDWAREKEYBOARD }

	public class SettingsData: ScriptableObject
	{
		[Tooltip("The behaviour to determine which keyboard type to use")]
		[SerializeField, CustomName("Keyboard Behaviour")]
		private MobileKeyboardBehaviour mobileKeyboardBehaviour;

		[Tooltip("The prefab to use for Action Bar")]
		[SerializeField, CustomName("Action Bar Prefab")]
		private ActionBar mobileActionBarPrefab;

		[Tooltip("The prefab to use for Selection Cursors")]
		[SerializeField, CustomName("Selection Cursors Prefab")]
		private Transform mobileSelectionCursorsPrefab;

		[Tooltip("The scale of the selection cursors (1 is default)")]
		[SerializeField, CustomName("Selection Cursors Scale")]
		[Range(0.01f, 10)]
		private float mobileSelectionCursorsScale = 1;

		public MobileKeyboardBehaviour MobileKeyboardBehaviour { get { return mobileKeyboardBehaviour; } }
		public ActionBar MobileActionBarPrefab { get { return mobileActionBarPrefab; } }
		public Transform MobileSelectionCursorsPrefab { get { return mobileSelectionCursorsPrefab; } }
		public float MobileSelectionCursorsScale { get { return mobileSelectionCursorsScale; } }
	}
}