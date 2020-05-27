//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Utility class with helper methods</summary>
	public class Util
	{
		/// <summary>The average thumb size of the user in inches</summary>
		private const float PHYSICAL_THUMB_SIZE = 1;

		public static float DeterminePhysicalScreenSize()
		{
			if(Screen.dpi <= 0)
			{
				return -1;
			}

			float width = Screen.width / Screen.dpi;
			float height = Screen.height / Screen.dpi;
			float screenSize = Mathf.Sqrt(Mathf.Pow(width, 2) + Mathf.Pow(height, 2));

			return screenSize;
		}

		/// <summary>The thumb size in screen pixels (diagonal)</summary>
		public static int DetermineThumbSize()
		{
			float physicalScreenSize = DeterminePhysicalScreenSize();
			if(physicalScreenSize <= 0)
			{
				return -1;
			}
			else
			{
				float normalizedThumbSize = (PHYSICAL_THUMB_SIZE / physicalScreenSize);
				float pixelScreenSize = Mathf.Sqrt(Mathf.Pow(Screen.width, 2) + Mathf.Pow(Screen.height, 2));
				float pixels = (pixelScreenSize * normalizedThumbSize) / 2f;

				return Mathf.RoundToInt(pixels);
			}
		}

		public static bool RectTransformIntersects(RectTransform rectTransform1, RectTransform rectTransform2)
		{
			Vector3[] corners1 = new Vector3[4]; //BottomLeft, TopLeft, TopRight, BottomRight
			rectTransform1.GetWorldCorners(corners1);

			Vector3[] corners2 = new Vector3[4]; //BottomLeft, TopLeft, TopRight, BottomRight
			rectTransform2.GetWorldCorners(corners2);

			Vector2 min = corners1[0];
			Vector2 max = corners1[2];

			int length = corners2.Length;
			int outOfBoundsCount = 0;
			for(int i = 0; i < length; i++)
			{
				Vector2 point = corners2[i];

				if(point.x < min.x || point.x > max.x || point.y < min.y || point.y > max.y)
				{
					outOfBoundsCount++;
				}
				else
				{
					break;
				}
			}

			return (outOfBoundsCount != length);
		}

		public static bool Contains(char ch, char[] text, int textLength)
		{
			for(int i = 0; i < textLength; i++)
			{
				if(text[i] == ch) { return true; }
			}

			return false;
		}

		public static int IndexOf(char ch, char[] text, int textLength)
		{
			for(int i = 0; i < textLength; i++)
			{
				if(text[i] == ch) { return i; }
			}

			return -1;
		}

		public static int LastIndexOf(char ch, char[] text, int textLength)
		{
			for(int i = textLength - 1; i >= 0; i++)
			{
				if(text[i] == ch) { return i; }
			}

			return -1;
		}

		public static int CountOccurences(char ch, char[] text, int textLength)
		{
			int occurences = 0;

			for(int i = 0; i < textLength; i++)
			{
				if(text[i] == ch)
				{
					occurences++;
				}
			}

			return occurences;
		}

		public static void StringCopy(ref char[] destination, string source)
		{
			int length = source.Length;
			for(int i = 0; i < length; i++)
			{
				destination[i] = source[i];
			}
		}
	}
}
