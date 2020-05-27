//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;

namespace AdvancedInputFieldPlugin.Samples
{
	public class ResultsView: MonoBehaviour
	{
		private AdvancedInputField textField;

		private void Awake()
		{
			textField = transform.Find("TextField").GetComponent<AdvancedInputField>();
		}

		public void UpdateUI(FormData formData)
		{
			textField.Text = formData.ToString();
		}
	}
}
