using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin
{
	[RequireComponent(typeof(Text))]
	public class UnityTextRenderer: TextRenderer
	{
		private new Text renderer;
		private Canvas canvas;

		public Text Renderer
		{
			get
			{
				if(renderer == null)
				{
					renderer = GetComponent<Text>();
				}

				return renderer;
			}
		}

		public override string Text
		{
			get
			{
				return Renderer.text;
			}
			set
			{
				Renderer.text = value;
				UpdateImmediately();
			}
		}

		public override bool Visible { get { return Renderer.enabled; } }

		public Canvas Canvas
		{
			get
			{
				if(canvas == null)
				{
					canvas = GetComponentInParent<Canvas>();
				}

				return canvas;
			}
		}

		public float CanvasScaleFactor
		{
			get
			{
				if(Canvas != null)
				{
					return Canvas.scaleFactor;
				}

				return 1;
			}
		}

		public override bool Multiline
		{
			get { return multiline; }
			set
			{
				multiline = value;
			}
		}

		public override TextAlignment TextAlignment
		{
			get
			{
				switch(renderer.alignment)
				{
					case TextAnchor.UpperLeft: return TextAlignment.TOP_LEFT;
					case TextAnchor.UpperCenter: return TextAlignment.TOP;
					case TextAnchor.UpperRight: return TextAlignment.TOP_RIGHT;
					case TextAnchor.MiddleLeft: return TextAlignment.LEFT;
					case TextAnchor.MiddleCenter: return TextAlignment.CENTER;
					case TextAnchor.MiddleRight: return TextAlignment.RIGHT;
					case TextAnchor.LowerLeft: return TextAlignment.BOTTOM_LEFT;
					case TextAnchor.LowerCenter: return TextAlignment.BOTTOM;
					case TextAnchor.LowerRight: return TextAlignment.BOTTOM_RIGHT;
					default: return TextAlignment.TOP_LEFT;
				}
			}
		}

		public override int LineCount
		{
			get
			{
				return Renderer.cachedTextGenerator.lineCount;
			}
		}

		public override int CharacterCount
		{
			get
			{
				return Renderer.cachedTextGenerator.characterCount;
			}
		}

		public override int CharacterCountVisible
		{
			get
			{
				return Renderer.cachedTextGenerator.characterCountVisible;
			}
		}

		public override float FontSize
		{
			get
			{
				return Renderer.fontSize;
			}
		}

		public override bool ResizeTextForBestFit
		{
			get
			{
				return Renderer.resizeTextForBestFit;
			}
		}

		public override float FontSizeUsedForBestFit
		{
			get
			{
				return Renderer.cachedTextGenerator.fontSizeUsedForBestFit;
			}
		}

		protected override void Awake()
		{
			base.Awake();

			renderer = GetComponent<Text>();
		}

		public override void Show()
		{
			Renderer.enabled = true;
		}

		public override void Hide()
		{
			Renderer.enabled = false;
		}

		public override void UpdateImmediately(bool generateOutOfBounds = true)
		{
			if(string.IsNullOrEmpty(Text))
			{
				return;
			}
			else
			{
				renderer.UpdateImmediately(generateOutOfBounds);
				RefreshCharacterData();
			}
			RefreshCharacterData();
		}

		public override void UpdateImmediately(string text, bool generateOutOfBounds = true)
		{
			if(string.IsNullOrEmpty(text))
			{
				return;
			}
			else
			{
				renderer.UpdateImmediately(text, generateOutOfBounds);
				RefreshCharacterData();
			}
			RefreshCharacterData();
		}

		public void RefreshCharacterData()
		{
			TextGenerator textGenerator = renderer.cachedTextGenerator;
			IList<UICharInfo> characters = textGenerator.characters;
			Vector2 rectSize = renderer.rectTransform.rect.size;
			float textWidth = 0;

			int length = characters.Count;
			for(int i = 0; i < length; i++)
			{
				UICharInfo charInfo = characters[i];

				if(!Multiline)
				{
					textWidth += (charInfo.charWidth / CanvasScaleFactor);
				}
			}

			if(!Multiline)
			{
				preferredSize.x = textWidth + CaretWidth;
			}
			else
			{
				RectTransform textAreaTransform = renderer.transform.parent.parent.GetComponent<RectTransform>();
				preferredSize.x = textAreaTransform.rect.width;
			}

			preferredSize.y = renderer.preferredHeight;
		}

		public override bool FontHasCharacter(char c)
		{
			return renderer.font.HasCharacter(c);
		}

		public override bool IsReady()
		{
			return renderer.IsReady();
		}

		public override CharacterInfo GetCharacterInfo(int index)
		{
			TextGenerator textGenerator = renderer.cachedTextGenerator;
			if(textGenerator.characterCount == 0)
			{
				UpdateImmediately(" ");
				UpdateImmediately("");
			}

			UICharInfo charInfo = textGenerator.characters[index];
			Vector2 rectSize = renderer.rectTransform.rect.size;
			CharacterInfo characterInfo = new CharacterInfo
			{
				position = charInfo.cursorPos / CanvasScaleFactor,
				width = charInfo.charWidth / CanvasScaleFactor
			};
			characterInfo.position.x += (rectSize.x * 0.5f);
			characterInfo.position.y -= (rectSize.y * 0.5f);

			return characterInfo;
		}

		public override LineInfo GetLineInfo(int index)
		{
			TextGenerator textGenerator = renderer.cachedTextGenerator;
			UILineInfo info = textGenerator.lines[index];
			Vector2 rectSize = renderer.rectTransform.rect.size;
			LineInfo lineInfo = new LineInfo
			{
				topY = (info.topY / CanvasScaleFactor) - (rectSize.y * 0.5f),
				height = info.height / CanvasScaleFactor,
				startCharIdx = info.startCharIdx
			};

			return lineInfo;
		}

		public override int GetLineEndCharIndex(int line)
		{
			TextGenerator textGenerator = renderer.cachedTextGenerator;
			line = Mathf.Max(line, 0);
			if(line + 1 < textGenerator.lineCount)
			{
				return textGenerator.lines[line + 1].startCharIdx - 1;
			}

			return textGenerator.characterCountVisible;
		}
	}
}
