using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	public struct CharacterInfo
	{
		public Vector2 position;
		public float width;
		public int index;
		public int partIndex;
		public int partCount;
	}

	public struct LineInfo
	{
		public float topY;
		public float height;
		public int startCharIdx;
	}

	public enum TextAlignment { TOP_LEFT, TOP, TOP_RIGHT, LEFT, CENTER, RIGHT, BOTTOM_LEFT, BOTTOM, BOTTOM_RIGHT };

	[RequireComponent(typeof(RectTransform))]
	public abstract class TextRenderer: MonoBehaviour
	{
		protected RectTransform rectTransform;
		protected Vector2 preferredSize;
		protected bool multiline;

		public RectTransform RectTransform
		{
			get
			{
				if(rectTransform == null)
				{
					rectTransform = GetComponent<RectTransform>();
				}

				return rectTransform;
			}
		}

		public abstract bool Visible { get; }
		public float CaretWidth { get; set; }
		public abstract bool Multiline { get; set; }
		public abstract TextAlignment TextAlignment { get; }

		public abstract string Text { get; set; }

		public Vector2 PreferredSize { get { return preferredSize; } }

		public abstract int LineCount { get; }
		public abstract int CharacterCount { get; }
		public abstract int CharacterCountVisible { get; }
		public abstract float FontSize { get; }
		public abstract bool ResizeTextForBestFit { get; }
		public abstract float FontSizeUsedForBestFit { get; }

		protected virtual void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		public abstract void Show();
		public abstract void Hide();

		public abstract void UpdateImmediately(bool generateOutOfBounds = true);
		public abstract void UpdateImmediately(string text, bool generateOutOfBounds = true);
		public abstract bool FontHasCharacter(char c);
		public abstract bool IsReady();
		public abstract CharacterInfo GetCharacterInfo(int index);
		public abstract LineInfo GetLineInfo(int index);

		/// <summary>Gets the character index  of the line end</summary>
		/// <param name="line">The line to check</param>
		public abstract int GetLineEndCharIndex(int line);
	}
}
