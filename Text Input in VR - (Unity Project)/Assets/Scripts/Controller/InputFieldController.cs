using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldController : MonoBehaviour
{
    public InputField mInput;
    // Start is called before the first frame update
    void Start()
    {
        mInput.shouldHideMobileInput = true;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (char c in Input.inputString)
        {
            mInput.Select();
            mInput.ActivateInputField();
            mInput.touchScreenKeyboard.active = false;
            TouchScreenKeyboard.hideInput = true;
        }
    }
    
}
