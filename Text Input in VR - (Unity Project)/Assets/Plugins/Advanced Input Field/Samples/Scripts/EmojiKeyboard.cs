using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin.Samples
{
	public class EmojiKeyboard: MonoBehaviour
	{
		[SerializeField]
		private Texture2D emojiAtlas;

		[SerializeField]
		private TextAsset emojiAtlasData;

		[SerializeField]
		private string emojiAtlasName;

		[SerializeField]
		private InputFieldButton emojiButtonPrefab;

		private EmojiCollection emojiCollection;
		private List<InputFieldButton> buttons;

#if UNITY_EDITOR
		private void OnValidate()
		{
			if(string.IsNullOrEmpty(emojiAtlasName)) { return; }

			string[] textureGuids = AssetDatabase.FindAssets(emojiAtlasName + " t:Texture2D");
			if(textureGuids.Length > 0)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(textureGuids[0]);
				emojiAtlas = (Texture2D)AssetDatabase.LoadAssetAtPath(assetPath, typeof(Texture2D));
			}

			string[] textGuids = AssetDatabase.FindAssets(emojiAtlasName + " t:TextAsset");
			if(textGuids.Length > 0)
			{
				string assetPath = AssetDatabase.GUIDToAssetPath(textGuids[0]);
				emojiAtlasData = (TextAsset)AssetDatabase.LoadAssetAtPath(assetPath, typeof(TextAsset));
			}
		}
#endif

		private void Awake()
		{
			if(emojiAtlasData != null)
			{
				InitializeButtons();
			}
		}

		private void InitializeButtons()
		{
			buttons = new List<InputFieldButton>();
			string json = emojiAtlasData.text;
			emojiCollection = JsonUtility.FromJson<EmojiCollection>(json);
			EmojiFrame[] emojiFrames = emojiCollection.frames;
			int length = emojiFrames.Length;

			RectTransform rectTransform = GetComponent<RectTransform>();
			Vector2 rectSize = rectTransform.rect.size;
			Vector2 emojiButtonSize = emojiButtonPrefab.GetComponent<RectTransform>().rect.size;
			int columns = Mathf.FloorToInt(rectSize.x / emojiButtonSize.x);
			int rows = Mathf.CeilToInt((float)length / (float)columns);

			for(int y = 0; y < rows; y++)
			{
				for(int x = 0; x < columns; x++)
				{
					int index = (y * columns) + x;
					if(index >= length) { continue; }

					EmojiFrame emojiFrame = emojiFrames[index];

					InputFieldButton emojiButton = CreateEmojiButton();
					emojiButton.onClick.AddListener(() => OnClick(index));
					RectTransform buttonTransform = emojiButton.GetComponent<RectTransform>();
					Vector2 position = new Vector2(x * emojiButtonSize.x, -(y * emojiButtonSize.y));
					buttonTransform.anchoredPosition = position;

					RawImage iconRenderer = emojiButton.GetComponentInChildren<RawImage>();
					iconRenderer.texture = emojiAtlas;
					float width = (float)emojiFrame.frame.w / (float)emojiAtlas.width;
					float height = (float)emojiFrame.frame.h / (float)emojiAtlas.height;
					float xOffset = (float)emojiFrame.frame.x / (float)emojiAtlas.width;
					float yOffset = (float)emojiFrame.frame.y / (float)emojiAtlas.height;
					iconRenderer.uvRect = new Rect(xOffset, 1 - yOffset - height, width, height);

					buttons.Add(emojiButton);
				}
			}

			float totalHeight = rows * emojiButtonSize.y;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			sizeDelta.y = totalHeight;
			rectTransform.sizeDelta = sizeDelta;
		}

		private InputFieldButton CreateEmojiButton()
		{
			InputFieldButton emojiButton = Instantiate(emojiButtonPrefab);
			RectTransform buttonTransform = emojiButton.GetComponent<RectTransform>();
			Vector2 size = buttonTransform.sizeDelta;
			buttonTransform.SetParent(transform);
			buttonTransform.localScale = Vector3.one;
			buttonTransform.localRotation = Quaternion.identity;
			buttonTransform.localPosition = Vector3.zero;
			buttonTransform.sizeDelta = size;

			return emojiButton;
		}

		public void OnClick(int index)
		{
			EmojiFrame emojiFrame = emojiCollection.frames[index];
			string emojiName = Path.GetFileNameWithoutExtension(emojiFrame.filename);
			string text = Char.ConvertFromUtf32(Int32.Parse(emojiName, NumberStyles.HexNumber));
			InputMethodManager.AddTextInputEvent(text);
		}
	}
}
