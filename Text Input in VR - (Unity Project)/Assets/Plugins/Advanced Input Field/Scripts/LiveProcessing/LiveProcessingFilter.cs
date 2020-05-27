//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using System;
using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Class to format text whenever the text of caret position changes</summary>
	[Serializable]
	public abstract class LiveProcessingFilter: MonoBehaviour
	{
		/// <summary>Formats text in a specific way</summary>
		/// <param name="text">The current (not processed) text value</param>
		/// <param name="caretPosition">The current caret position in the (not processed) text</param>
		/// <returns>The processed text</returns>
		public abstract string ProcessText(string text, int caretPosition);

		/// <summary>Determines the correct caret position in the processed text</summary>
		/// <param name="text">The current (not processed) text value</param>
		/// <param name="caretPosition">The current caret position in the (not processed) text</param>
		/// <param name="processedText">The current processed text value</param>
		/// <returns>The caret position in the processed text</returns>
		public abstract int DetermineProcessedCaret(string text, int caretPosition, string processedText);

		/// <summary>Determines the correct caret position in the (not processed) text</summary>
		/// <param name="text">The current (not processed) text value</param>
		/// <param name="processedText">The current processed text value</param>
		/// <param name="processedCaretPosition">The current caret position in the processed text</param>
		/// <returns>The caret position in the processed text</returns>
		public abstract int DetermineCaret(string text, string processedText, int processedCaretPosition);
	}
}
