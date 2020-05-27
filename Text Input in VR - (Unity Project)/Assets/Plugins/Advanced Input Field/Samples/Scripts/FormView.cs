//-----------------------------------------
//			Advanced Input Field
// Copyright (c) 2017 Jeroen van Pienbroek
//------------------------------------------

using UnityEngine;
using UnityEngine.UI;

namespace AdvancedInputFieldPlugin.Samples
{
	public class FormView: MonoBehaviour
	{
		private AdvancedInputField usernameInput;
		private AdvancedInputField passwordInput;
		private AdvancedInputField emailInput;
		private AdvancedInputField telephoneInput;
		private AdvancedInputField firstNameInput;
		private AdvancedInputField lastNameInput;
		private AdvancedInputField countryInput;
		private AdvancedInputField cityInput;
		private AdvancedInputField yearlyIncomeInput;
		private AdvancedInputField hourlyWageInput;
		private AdvancedInputField commentsInput;
		private Button submitButton;

		public string Username { get { return usernameInput.Text; } }
		public string Password { get { return passwordInput.Text; } }
		public string Email { get { return emailInput.Text; } }
		public string Telephone { get { return telephoneInput.Text; } }
		public string FirstName { get { return firstNameInput.Text; } }
		public string LastName { get { return lastNameInput.Text; } }
		public string Country { get { return countryInput.Text; } }
		public string City { get { return cityInput.Text; } }
		public string YearlyIncome { get { return yearlyIncomeInput.Text; } }
		public string HourlyWage { get { return hourlyWageInput.Text; } }
		public string Comments { get { return commentsInput.Text; } }

		private void Awake()
		{
			ScrollRect scrollRect = transform.Find("ScrollView").GetComponent<ScrollRect>();
			Transform centerGroup = scrollRect.content.Find("CenterGroup");
			usernameInput = centerGroup.Find("Username").GetComponentInChildren<AdvancedInputField>();
			passwordInput = centerGroup.Find("Password").GetComponentInChildren<AdvancedInputField>();
			emailInput = centerGroup.Find("Email").GetComponentInChildren<AdvancedInputField>();
			telephoneInput = centerGroup.Find("Telephone").GetComponentInChildren<AdvancedInputField>();
			firstNameInput = centerGroup.Find("FirstName").GetComponentInChildren<AdvancedInputField>();
			lastNameInput = centerGroup.Find("LastName").GetComponentInChildren<AdvancedInputField>();
			countryInput = centerGroup.Find("Country").GetComponentInChildren<AdvancedInputField>();
			cityInput = centerGroup.Find("City").GetComponentInChildren<AdvancedInputField>();
			yearlyIncomeInput = centerGroup.Find("YearlyIncome").GetComponentInChildren<AdvancedInputField>();
			hourlyWageInput = centerGroup.Find("HourlyWage").GetComponentInChildren<AdvancedInputField>();
			commentsInput = centerGroup.Find("Comments").GetComponentInChildren<AdvancedInputField>();
			submitButton = centerGroup.Find("SubmitButton").GetComponent<Button>();
		}

		private void OnEnable()
		{
			usernameInput.Clear();
			passwordInput.Clear();
			emailInput.Clear();
			telephoneInput.Clear();
			firstNameInput.Clear();
			lastNameInput.Clear();
			countryInput.Clear();
			cityInput.Clear();
			yearlyIncomeInput.Clear();
			hourlyWageInput.Clear();
			commentsInput.Clear();
			submitButton.interactable = false;
		}

		public void EnableSubmitButton()
		{
			submitButton.interactable = true;
		}

		public void DisableSubmitButton()
		{
			submitButton.interactable = false;
		}
	}
}