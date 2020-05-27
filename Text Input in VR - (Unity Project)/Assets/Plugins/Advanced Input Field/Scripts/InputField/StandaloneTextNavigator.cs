//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;

namespace AdvancedInputFieldPlugin
{
	/// <summary>Subclass of TextNavigator for Standalone platforms</summary>
	public class StandaloneTextNavigator: TextNavigator
	{
		/// <summary>Handles the left arrow key</summary>
		/// <param name="shift">Indicates if shift is pressed</param>
		/// <param name="ctrl">Indicates if ctrl is pressed</param>
		internal void MoveLeft(bool shift, bool ctrl)
		{
			if(ctrl)
			{
				MoveCtrlLeft();
			}
			else if(shift)
			{
				MoveShiftLeft();
			}
			else
			{
				MoveLeft();
			}
		}

		/// <summary>Handles the left arrow + ctrl key combination</summary>
		internal void MoveCtrlLeft()
		{
			if(HasSelection)
			{
				int result = FindPreviousWordStart(SelectionStartPosition, Text);
				CaretPosition = result;
				CancelSelection();
			}
			else
			{
				int result = FindPreviousWordStart(CaretPosition, Text);
				CaretPosition = result;
			}
		}

		/// <summary>Handles the left arrow + shift key combination</summary>
		internal void MoveShiftLeft()
		{
			if(HasSelection)
			{
				if(CaretPosition == SelectionStartPosition)
				{
					CaretPosition = Mathf.Max(0, CaretPosition - 1);
					SelectionStartPosition = CaretPosition;
				}
				else
				{
					CaretPosition = Mathf.Max(0, CaretPosition - 1);
					SelectionEndPosition = CaretPosition;
				}
			}
			else
			{
				SelectionEndPosition = CaretPosition;
				CaretPosition = Mathf.Max(0, CaretPosition - 1);
				SelectionStartPosition = CaretPosition;
			}
		}

		/// <summary>Handles the left arrow (without ctrl and shift)</summary>
		internal void MoveLeft()
		{
			if(HasSelection)
			{
				CaretPosition = SelectionStartPosition;
				SelectionEndPosition = SelectionStartPosition; //Cancel selection
			}
			else
			{
				CaretPosition = Mathf.Max(0, CaretPosition - 1);
			}
		}

		/// <summary>Handles the right arrow key</summary>
		/// <param name="shift">Indicates if shift is pressed</param>
		/// <param name="ctrl">Indicates if ctrl is pressed</param>
		internal void MoveRight(bool shift, bool ctrl)
		{
			if(ctrl)
			{
				MoveCtrlRight();
			}
			else if(shift)
			{
				MoveShiftRight();
			}
			else
			{
				MoveRight();
			}
		}

		/// <summary>Handles the right arrow + ctrl key combination</summary>
		internal void MoveCtrlRight()
		{
			if(HasSelection)
			{
				int result = FindNextWordStart(SelectionEndPosition, Text);
				CaretPosition = result;
				CancelSelection();
			}
			else
			{
				int result = FindNextWordStart(CaretPosition, Text);
				CaretPosition = result;
			}
		}

		/// <summary>Handles the right arrow + shift key combination</summary>
		internal void MoveShiftRight()
		{
			if(HasSelection)
			{
				if(CaretPosition == SelectionStartPosition)
				{
					CaretPosition = Mathf.Min(TotalCharacterCount, CaretPosition + 1);
					SelectionStartPosition = CaretPosition;
				}
				else
				{
					CaretPosition = Mathf.Min(TotalCharacterCount, CaretPosition + 1);
					SelectionEndPosition = CaretPosition;
				}
			}
			else
			{
				SelectionStartPosition = CaretPosition;
				CaretPosition = Mathf.Min(TotalCharacterCount, CaretPosition + 1);
				SelectionEndPosition = CaretPosition;
			}
		}

		/// <summary>Handles the right arrow (without ctrl and shift)</summary>
		internal void MoveRight()
		{
			if(HasSelection)
			{
				CaretPosition = SelectionEndPosition;
				SelectionStartPosition = SelectionEndPosition; //Cancel selection
			}
			else
			{
				CaretPosition = Mathf.Min(TotalCharacterCount, CaretPosition + 1);
			}
		}

		/// <summary>Handles the down arrow key</summary>
		/// <param name="shift">Indicates if shift is pressed</param>
		/// <param name="ctrl">Indicates if ctrl is pressed</param>
		internal void MoveDown(bool shift, bool ctrl)
		{
			if(ctrl)
			{
				MoveCtrlDown();
			}
			else if(shift)
			{
				MoveShiftDown();
			}
			else
			{
				MoveDown();
			}
		}

		/// <summary>Handles the down arrow + ctrl key combination</summary>
		internal void MoveCtrlDown()
		{
			if(HasSelection)
			{
				CancelSelection();
			}

			CaretPosition = NewLineDownPosition(CaretPosition);
		}

		/// <summary>Handles the down arrow + shift key combination</summary>
		internal void MoveShiftDown()
		{
			int position = LineDownPosition(CaretPosition);

			if(HasSelection)
			{
				if(CaretPosition == SelectionStartPosition)
				{
					if(position <= SelectionEndPosition)
					{
						SelectionStartPosition = position;
						CaretPosition = position;
					}
					else
					{
						SelectionStartPosition = SelectionEndPosition; //Invert
						SelectionEndPosition = position;
						CaretPosition = position;
					}
				}
				else
				{
					if(position >= SelectionEndPosition)
					{
						SelectionEndPosition = position;
						CaretPosition = position;
					}
					else
					{
						SelectionEndPosition = SelectionStartPosition; //Invert
						SelectionStartPosition = position;
						CaretPosition = position;
					}
				}
			}
			else
			{
				SelectionStartPosition = CaretPosition;
				SelectionEndPosition = position;
				CaretPosition = position;
			}
		}

		/// <summary>Handles the down arrow (without ctrl and shift)</summary>
		internal void MoveDown()
		{
			if(HasSelection)
			{
				CancelSelection();
			}

			CaretPosition = LineDownPosition(CaretPosition);
		}

		/// <summary>Handles the up arrow key</summary>
		/// <param name="shift">Indicates if shift is pressed</param>
		/// <param name="ctrl">Indicates if ctrl is pressed</param>
		internal void MoveUp(bool shift, bool ctrl)
		{
			if(ctrl)
			{
				MoveCtrlUp();
			}
			else if(shift)
			{
				MoveShiftUp();
			}
			else
			{
				MoveUp();
			}
		}

		/// <summary>Handles the up arrow + ctrl key combination</summary>
		internal void MoveCtrlUp()
		{
			if(HasSelection)
			{
				CancelSelection();
			}

			CaretPosition = NewLineUpPosition(CaretPosition);
		}

		/// <summary>Handles the up arrow + shift key combination</summary>
		internal void MoveShiftUp()
		{
			int position = LineUpPosition(CaretPosition);

			if(HasSelection)
			{
				if(CaretPosition == SelectionStartPosition)
				{
					if(position <= SelectionStartPosition)
					{
						SelectionStartPosition = position;
						CaretPosition = position;
					}
					else
					{
						SelectionStartPosition = SelectionEndPosition; //Invert
						SelectionEndPosition = position;
						CaretPosition = position;
					}
				}
				else
				{
					if(position >= SelectionStartPosition)
					{
						SelectionEndPosition = position;
						CaretPosition = position;
					}
					else
					{
						SelectionEndPosition = SelectionStartPosition; //Invert
						SelectionStartPosition = position;
						CaretPosition = position;
					}
				}
			}
			else
			{
				SelectionStartPosition = position;
				SelectionEndPosition = CaretPosition;
				CaretPosition = position;
			}
		}

		/// <summary>Handles the up arrow (without ctrl and shift)</summary>
		internal void MoveUp()
		{
			if(HasSelection)
			{
				CancelSelection();
			}

			CaretPosition = LineUpPosition(CaretPosition);
		}

		/// <summary>Selects the next InputField(if any)</summary>
		internal void GoToNextInputField()
		{
			if(InputField.NextInputField != null)
			{
				InputField.Deselect(EndEditReason.KEYBOARD_NEXT);
				InputField.NextInputField.ManualSelect(BeginEditReason.KEYBOARD_NEXT);
			}
		}
	}
}