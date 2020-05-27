//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEditor;

namespace AdvancedInputFieldPlugin.Editor
{
	[CustomEditor(typeof(SettingsData), true)]
	public class SettingsDataEditor: UnityEditor.Editor
	{
		private SerializedProperty mobileKeyboardBehaviourProperty;
		private SerializedProperty mobileActionBarPrefabProperty;
		private SerializedProperty mobileSelectionCursorsPrefabProperty;
		private SerializedProperty mobileSelectionCursorsScaleProperty;

		private void OnEnable()
		{
			mobileKeyboardBehaviourProperty = serializedObject.FindProperty("mobileKeyboardBehaviour");
			mobileActionBarPrefabProperty = serializedObject.FindProperty("mobileActionBarPrefab");
			mobileSelectionCursorsPrefabProperty = serializedObject.FindProperty("mobileSelectionCursorsPrefab");
			mobileSelectionCursorsScaleProperty = serializedObject.FindProperty("mobileSelectionCursorsScale");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			SettingsData settingsData = (SettingsData)target;

			EditorGUILayout.LabelField("Mobile:", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(mobileKeyboardBehaviourProperty);
			EditorGUILayout.PropertyField(mobileActionBarPrefabProperty);
			EditorGUILayout.PropertyField(mobileSelectionCursorsPrefabProperty);
			EditorGUILayout.PropertyField(mobileSelectionCursorsScaleProperty);

			serializedObject.ApplyModifiedProperties();
		}
	}
}