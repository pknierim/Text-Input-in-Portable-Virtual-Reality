using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phrases : MonoBehaviour, IPhrases
{
    public static string[] phrases;
    public static int phraseCounter = 0;
    public static int phraseNumber;
    public static List<string> allPhrases = new List<string>();
    public static bool gameStarted;

    protected static int NUMBER_OF_PHRASES = GlobalSettings.NumberOfPhrasesForWriting;
    protected ITextInputControllerListener textInputControllerListener;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (allPhrases.Count > 0) allPhrases.Clear();
        gameStarted = false;
        phraseCounter = 0;
        TextAsset phraseAsset = Resources.Load("phrases2") as TextAsset;
        phrases = phraseAsset.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

        for (int i = 0; i < NUMBER_OF_PHRASES; i++)
        {
            AddPhrase();
        }
    }

    private void AddPhrase()
    {
        int number = GenerateRandomPhraseNumber();

        if (!allPhrases.Contains(phrases[number]) && !GlobalSettings.UsedPhrases.Contains(number))
        {
            allPhrases.Add(phrases[number]); GlobalSettings.UsedPhrases.Add(number);
        }
        else { AddPhrase(); }
    }

    public void SetListenerTextInput(ITextInputControllerListener l)
    {
        this.textInputControllerListener = l;
    }

    private void NextSentence()
    {
        if (phraseCounter == allPhrases.Count)  // allPhrases.Count - 1
        {
            textInputControllerListener.OnFinished(false);
        }
        else
        {
            string nextPhrase = allPhrases[phraseCounter];
            if (nextPhrase.Length == 0) { AddPhrase(); phraseCounter++; NextSentence(); return; }   // TODO: double check if safe from endless loop
            if (phraseCounter < allPhrases.Count) textInputControllerListener.NextSentence(allPhrases[phraseCounter]);
            phraseCounter++;
        }
    }

    private void FirstSentence()
    {
        NextSentence();
    }

    private int GenerateRandomPhraseNumber()
    {
        phraseNumber = UnityEngine.Random.Range(0, phrases.Length);
        return phraseNumber;
    }

    private string[] ReadInTexts(string path)
    {
        return System.IO.File.ReadAllLines(path);
    }

    public void TextFinished()
    {
        NextSentence();
    }

    public void OnStartTask()
    {
        FirstSentence();
    }
}

public interface IPhrases
{
    void TextFinished();
    void OnStartTask();
}
