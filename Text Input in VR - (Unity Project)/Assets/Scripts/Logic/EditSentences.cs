using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditSentences : MonoBehaviour, IEditSentences
{
    protected ITextEditControllerListener listener;
    protected IControllerLogic controllerLogic;
    public int textCounter = 0;
    protected string UserID;
    protected int PackageNumber;
    protected TextPackageLogic logic;
    public static string CurrentText;
    public static TextPackage CurrentPackage;

    public void Start()
    {
        textCounter = 0;
        logic = GetComponent<TextPackageLogic>();
        logic.Init();
        //Debug.Log("EditSentences - Start - TPNo - " + GlobalSettings.TextPackageNumber);
        CurrentPackage = logic.Packages[GlobalSettings.UserID][GlobalSettings.TextPackageNumber];
    }

    private void NextText(bool isFirst)
    {
        //string textWithErrors = isFirst ? CurrentPackage.TextObjects[textCounter].GetTextWithErrors() :
        //                                                CurrentPackage.TextObjects[textCounter].GetTextWithErrors() + Environment.NewLine + 
        //                                                CurrentPackage.TextObjects[textCounter + 1].GetTextWithErrors();
        //string textWithCorr = isFirst ? CurrentPackage.TextObjects[textCounter].GetTextWithCorrections() :
        //CurrentPackage.TextObjects[textCounter].GetTextWithCorrections() + Environment.NewLine +
        //CurrentPackage.TextObjects[textCounter + 1].GetTextWithCorrections();
        string textWithErrors = GlobalSettings.TextLoad != 0 ? CurrentPackage.TextObjects[textCounter].GetTextWithErrors() + Environment.NewLine +
                                              CurrentPackage.TextObjects[textCounter + 1].GetTextWithErrors() :
                                              CurrentPackage.TextObjects[textCounter].GetTextWithErrors();
        string textWithCorr = GlobalSettings.TextLoad != 0 ? CurrentPackage.TextObjects[textCounter].GetTextWithCorrections() + Environment.NewLine +
                                            CurrentPackage.TextObjects[textCounter + 1].GetTextWithCorrections() :
                                            CurrentPackage.TextObjects[textCounter].GetTextWithCorrections();

        CurrentText = isFirst ? CurrentPackage.TextObjects[textCounter].GetTextRaw() :
                                              CurrentPackage.TextObjects[textCounter].GetTextRaw() + Environment.NewLine +
                                              CurrentPackage.TextObjects[textCounter + 1].GetTextRaw();
        listener.NextText(textWithErrors, textWithCorr);
        textCounter = GlobalSettings.TextLoad != 0 ? textCounter + GlobalSettings.TextLoad : textCounter + 1;      
        //isFirst ? textCounter + 1 : textCounter + GlobalSettings.TextLoad;
    }

    public void SetListenerControllerLogic(IControllerLogic l)
    {
        this.controllerLogic = l;
    }

    public void SetEditControllerListener(ITextEditControllerListener l)
    {
        this.listener = l;
    }

    public void OnTextFinished()
    {
        Debug.Log("EditSentences - OnTextFinished");
        NextText(false);
    }

    public void OnEditStarted(bool isFirst)
    {
        Debug.Log("EditSentences - OnEditStarted - isFirst ? " + isFirst);
        NextText(isFirst);
    }
}

public interface IEditSentences 
{
    void OnTextFinished();
    void OnEditStarted(bool isFirst);
}
