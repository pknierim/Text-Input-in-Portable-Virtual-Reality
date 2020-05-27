﻿//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Renderer for text selection</summary>
	[ExecuteInEditMode]
	[Serializable]
	public class TextSelectionRenderer: MaskableGraphic
	{
		/// <summary>The start position of the selection</summary>
		private int startPosition;

		/// <summary>The end position of the selection</summary>
		private int endPosition;

		private AdvancedInputField InputField { get; set; }

		public TextRenderer TextRenderer
		{
			get
			{
				if(InputField.LiveProcessing)
				{
					return InputField.ProcessedTextRenderer;
				}
				else
				{
					return InputField.TextRenderer;
				}
			}
		}

		internal void Initialize(AdvancedInputField inputField)
		{
			InputField = inputField;
		}

		/// <summary>Updates the selection</summary>
		/// <param name="startPosition">The start position of the selection</param>
		/// <param name="endPosition">The end position of the selection</param>
		internal void UpdateSelection(int startPosition, int endPosition, bool forceUpdate = false)
		{
			startPosition = Mathf.Clamp(startPosition, 0, TextRenderer.CharacterCount - 1);
			endPosition = Mathf.Clamp(endPosition, 0, TextRenderer.CharacterCount - 1);

			if(forceUpdate || this.startPosition != startPosition || this.endPosition != endPosition) //Only redraw if selection has changed
			{
				this.startPosition = startPosition;
				this.endPosition = endPosition;
				SetVerticesDirty();
			}
		}

		protected override void OnPopulateMesh(VertexHelper vertexHelper)
		{
			vertexHelper.Clear();

			if(startPosition == -1 || endPosition == -1 || startPosition == endPosition)
			{
				return;
			}

			int length = TextRenderer.LineCount;
			bool foundStart = false;
			bool foundEnd = false;
			for(int i = 0; i < length; i++)
			{
				LineInfo lineInfo = TextRenderer.GetLineInfo(i);

				int startCharIndex = GetLineStartCharIndex(i);
				int endCharIndex = GetLineEndCharIndex(i);
				if(endCharIndex < startPosition)
				{
					continue;
				}

				CharacterInfo startCharInfo;
				CharacterInfo endCharInfo;

				if(!foundStart)
				{
					foundStart = true;
					startCharInfo = TextRenderer.GetCharacterInfo(startPosition);
				}
				else
				{
					startCharInfo = TextRenderer.GetCharacterInfo(startCharIndex);
				}

				if(endPosition <= endCharIndex)
				{
					foundEnd = true;
					endCharInfo = TextRenderer.GetCharacterInfo(endPosition);
				}
				else
				{
					endCharInfo = TextRenderer.GetCharacterInfo(endCharIndex);
				}

				Rect boundsRect = InputField.TextContentTransform.rect;
				Vector2 position = InputField.TextContentTransform.anchoredPosition;

				Vector2 bottomLeftCorner = startCharInfo.position;
				bottomLeftCorner.x -= (boundsRect.size.x * 0.5f);
				bottomLeftCorner.y += (boundsRect.size.y * 0.5f);
				bottomLeftCorner.y -= lineInfo.height;

				Vector2 topRightCorner = endCharInfo.position;
				topRightCorner.x -= (boundsRect.size.x * 0.5f);
				topRightCorner.y += (boundsRect.size.y * 0.5f);

				AddRectangle(bottomLeftCorner, topRightCorner, ref vertexHelper);

				if(foundEnd)
				{
					break;
				}
			}
		}

		/// <summary>Gets the start char position of the line</summary>
		/// <param name="line">The line to use</param>
		internal int GetLineStartCharIndex(int line)
		{
			line = Mathf.Clamp(line, 0, TextRenderer.LineCount - 1);
			return TextRenderer.GetLineInfo(line).startCharIdx;
		}

		/// <summary>Gets the end char position of the line</summary>
		/// <param name="line">The line to use</param>
		internal int GetLineEndCharIndex(int line)
		{
			line = Mathf.Max(line, 0);
			if(line + 1 < TextRenderer.LineCount)
			{
				return TextRenderer.GetLineInfo(line + 1).startCharIdx - 1;
			}

			return TextRenderer.CharacterCountVisible;
		}

		/// <summary>Adds a rectangle to VertexHelper</summary>
		/// <param name="bottomLeftCorner">The bottom left corner of the rectangle</param>
		/// <param name="topRightCorner">The top right corner of the rectangle</param>
		/// <param name="vertexHelper">The VertexHelper to use</param>
		internal void AddRectangle(Vector2 bottomLeftCorner, Vector2 topRightCorner, ref VertexHelper vertexHelper)
		{
			UIVertex vertex = UIVertex.simpleVert;

			vertex.position = new Vector2(bottomLeftCorner.x, bottomLeftCorner.y);
			vertex.color = color;
			vertexHelper.AddVert(vertex);

			vertex.position = new Vector2(bottomLeftCorner.x, topRightCorner.y);
			vertex.color = color;
			vertexHelper.AddVert(vertex);

			vertex.position = new Vector2(topRightCorner.x, topRightCorner.y);
			vertex.color = color;
			vertexHelper.AddVert(vertex);

			vertex.position = new Vector2(topRightCorner.x, bottomLeftCorner.y);
			vertex.color = color;
			vertexHelper.AddVert(vertex);

			int vertexCount = vertexHelper.currentVertCount;
			vertexHelper.AddTriangle(vertexCount - 4, vertexCount - 3, vertexCount - 2);
			vertexHelper.AddTriangle(vertexCount - 2, vertexCount - 1, vertexCount - 4);
		}
	}
}