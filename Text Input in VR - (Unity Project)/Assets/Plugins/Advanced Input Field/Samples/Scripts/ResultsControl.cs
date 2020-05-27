//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;

namespace AdvancedInputFieldPlugin.Samples
{
	public class ResultsControl: MonoBehaviour
	{
		[SerializeField]
		public FormControl formControl;

		[SerializeField]
		private ResultsView view;

		public FormData FormData { get; set; }

		private void OnEnable()
		{
			if(view != null)
			{
				view.gameObject.SetActive(true);
				view.UpdateUI(FormData);
			}
		}

		private void OnDisable()
		{
			if(view != null)
			{
				view.gameObject.SetActive(false);
			}
		}

		public void OnBackClick()
		{
			formControl.gameObject.SetActive(true);
			gameObject.SetActive(false);
		}
	}
}
