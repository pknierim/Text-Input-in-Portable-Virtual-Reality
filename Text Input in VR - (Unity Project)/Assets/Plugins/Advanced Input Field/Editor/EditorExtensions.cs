//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using System.IO;
using UnityEditor;
using UnityEngine;

namespace AdvancedInputFieldPlugin.Editor
{
	public class EditorExtensions
	{
		private const string INPUT_FIELD_UNITY_TEXT_PREFAB_PATH = "AdvancedInputField/Prefabs/AdvancedInputField_Unity_Text";
		private const string INPUT_FIELD_TMPRO_TEXT_PREFAB_PATH = "AdvancedInputField/Prefabs/AdvancedInputField_TextMeshPro_Text";
		private const string SETTINGS_PATH = "AdvancedInputField/Settings";

		[MenuItem("Advanced Input Field/Create/InputField with Unity Text")]
		public static void CreateInputFieldUnityText()
		{
			GameObject prefab = Resources.Load(INPUT_FIELD_UNITY_TEXT_PREFAB_PATH) as GameObject;
			CreateInstance(prefab);
		}

		[MenuItem("Advanced Input Field/Create/InputField with TextMeshPro Text")]
		public static void CreateInputFieldTMProText()
		{
			GameObject prefab = Resources.Load(INPUT_FIELD_TMPRO_TEXT_PREFAB_PATH) as GameObject;
			CreateInstance(prefab);
		}

		[MenuItem("Advanced Input Field/Create/Character Validator")]
		public static void CreateCharacterValidator()
		{
			CharacterValidator asset = ScriptableObject.CreateInstance<CharacterValidator>();

			string currentDirectoryPath = EditorUtil.GetCurrentDirectoryPath();
			string outputFilePath = Path.Combine(currentDirectoryPath, "CharacterValidator.asset");
			AssetDatabase.CreateAsset(asset, outputFilePath);
			AssetDatabase.SaveAssets();

			Selection.activeObject = asset;
		}

		[MenuItem("Advanced Input Field/Global Settings")]
		public static void OpenSettings()
		{
			Object settings = Resources.Load(SETTINGS_PATH);
			Selection.activeObject = settings;
		}

		[MenuItem("Advanced Input Field/About")]
		public static void OpenVersionInfo()
		{
			VersionInfoWindow window = (VersionInfoWindow)EditorWindow.GetWindowWithRect(typeof(VersionInfoWindow), new Rect(0, 0, 640, 480), true, VersionInfoWindow.TITLE);
			window.Initialize();
		}

		private static void CreateInstance(GameObject prefab)
		{
			GameObject instance = GameObject.Instantiate(prefab);

			Transform parentTransform = Selection.activeTransform;
			instance.transform.SetParent(parentTransform);
			instance.transform.localPosition = Vector3.zero;
			instance.transform.localScale = Vector3.one;

			string name = instance.name;
			name = name.Substring(0, name.Length - "(Clone)".Length);
			instance.name = name;
		}
	}
}