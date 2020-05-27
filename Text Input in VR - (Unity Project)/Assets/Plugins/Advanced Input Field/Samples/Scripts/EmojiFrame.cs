using System;

namespace AdvancedInputFieldPlugin.Samples
{
	[Serializable]
	public class EmojiRect
	{
		public int x, y, w, h;
	}

	[Serializable]
	public class EmojiSize
	{
		public int w, h;
	}

	[Serializable]
	public class EmojiVector2D
	{
		public float x, y;
	}

	[Serializable]
	public class EmojiFrame
	{
		public string filename;
		public EmojiRect frame;
		public bool rotated;
		public bool trimmed;
		public EmojiSize spriteSourceSize;
		public EmojiSize sourceSize;
		public EmojiVector2D pivot;
	}
}
