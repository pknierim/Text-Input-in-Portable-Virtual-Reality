//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin.Samples
{
	public class MultilineControl: MonoBehaviour
	{
		[SerializeField]
		private ScrollRect scrollRect;

		private void OnEnable()
		{
			scrollRect.verticalNormalizedPosition = 1;
		}
	}
}
