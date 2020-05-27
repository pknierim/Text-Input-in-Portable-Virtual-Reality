using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdvancedInputFieldPlugin;
using UnityEngine.EventSystems;

public class InputFieldTest : MonoBehaviour
{
    public AdvancedInputField inputField;

    void Start()
    {
        //GUI.enabled = false;
        inputField.OnEndEdit.AddListener(OnEnd);
        inputField.OnBeginEdit.AddListener(OnStart);
        inputField.Select();
        inputField.ManualDeselect();
        inputField.interactable = true;
        inputField.ShouldBlockDeselect = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !inputField.Selected)
        {
            inputField.Select();
            inputField.ManualSelect();
		}
        else if (Input.GetKeyDown(KeyCode.Return) && inputField.Selected)
		{
			inputField.ManualDeselect();
		}
    }
        
    void OnEnd(string reason, EndEditReason r)
    {
        Debug.Log("Test --- " + r);
    }

    void OnStart(BeginEditReason e)
    {
        Debug.Log("Test --- " + e);
        //inputField.ShouldBlockDeselect = true;
    }
}
