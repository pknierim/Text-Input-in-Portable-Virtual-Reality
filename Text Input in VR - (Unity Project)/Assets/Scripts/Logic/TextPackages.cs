using System;

public class TextPackages
{
    public static string NewLine = Environment.NewLine;

    public TextPackage tp1 = new TextPackage(new TextObject(TextsShort.twe1, TextsShort.twc1, TextsShort.twr1), 
                                                    new TextObject(TextsShort.twe2, TextsShort.twc2, TextsShort.twr2), 
                                                    new TextObject(TextsShort.twe3, TextsShort.twc3, TextsShort.twr3),
                                                    new TextObject(TextsShort.twe4, TextsShort.twc4, TextsShort.twr4),
                                                    new TextObject(TextsShort.twe5, TextsShort.twc5, TextsShort.twr5),
                                                    new TextObject(TextsShort.twe6, TextsShort.twc6, TextsShort.twr6),
                                                    new TextObject(TextsShort.twe7, TextsShort.twc7, TextsShort.twr7),
                                                    new TextObject(TextsShort.twe8, TextsShort.twc8, TextsShort.twr8),
                                                    new TextObject(TextsShort.twe9, TextsShort.twc9, TextsShort.twr9),
                                                    new TextObject(TextsShort.twe37, TextsShort.twc37, TextsShort.twr37));
    public TextPackage tp2 = new TextPackage(new TextObject(TextsShort.twe10, TextsShort.twc10, TextsShort.twr10),
                                                    new TextObject(TextsShort.twe11, TextsShort.twc11, TextsShort.twr11),
                                                    new TextObject(TextsShort.twe12, TextsShort.twc12, TextsShort.twr12),
                                                    new TextObject(TextsShort.twe13, TextsShort.twc13, TextsShort.twr13),
                                                    new TextObject(TextsShort.twe14, TextsShort.twc14, TextsShort.twr14),
                                                    new TextObject(TextsShort.twe15, TextsShort.twc15, TextsShort.twr15),
                                                    new TextObject(TextsShort.twe16, TextsShort.twc16, TextsShort.twr16),
                                                    new TextObject(TextsShort.twe17, TextsShort.twc17, TextsShort.twr17),
                                                    new TextObject(TextsShort.twe18, TextsShort.twc18, TextsShort.twr18),
                                                    new TextObject(TextsShort.twe38, TextsShort.twc38, TextsShort.twr38));
    public TextPackage tp3 = new TextPackage(new TextObject(TextsShort.twe19, TextsShort.twc19, TextsShort.twr19),
                                                    new TextObject(TextsShort.twe20, TextsShort.twc20, TextsShort.twr20),
                                                    new TextObject(TextsShort.twe21, TextsShort.twc21, TextsShort.twr21),
                                                    new TextObject(TextsShort.twe22, TextsShort.twc22, TextsShort.twr22),
                                                    new TextObject(TextsShort.twe23, TextsShort.twc23, TextsShort.twr23),
                                                    new TextObject(TextsShort.twe24, TextsShort.twc24, TextsShort.twr24),
                                                    new TextObject(TextsShort.twe25, TextsShort.twc25, TextsShort.twr25),
                                                    new TextObject(TextsShort.twe26, TextsShort.twc26, TextsShort.twr26),
                                                    new TextObject(TextsShort.twe27, TextsShort.twc27, TextsShort.twr27),
                                                    new TextObject(TextsShort.twe39, TextsShort.twc39, TextsShort.twr39));
    //public TextPackage tp4 = new TextPackage(new TextObject(TextsShort.twe28, TextsShort.twc28, TextsShort.twr28),
                                                    //new TextObject(TextsShort.twe29, TextsShort.twc29, TextsShort.twr29),
                                                    //new TextObject(TextsShort.twe30, TextsShort.twc30, TextsShort.twr30),
                                                    //new TextObject(TextsShort.twe31, TextsShort.twc31, TextsShort.twr31),
                                                    //new TextObject(TextsShort.twe32, TextsShort.twc32, TextsShort.twr32),
                                                    //new TextObject(TextsShort.twe33, TextsShort.twc33, TextsShort.twr33),
                                                    //new TextObject(TextsShort.twe34, TextsShort.twc34, TextsShort.twr34),
                                                    //new TextObject(TextsShort.twe35, TextsShort.twc35, TextsShort.twr35),
                                                    //new TextObject(TextsShort.twe36, TextsShort.twc36, TextsShort.twr36),
                                                    //new TextObject(TextsShort.twe40, TextsShort.twc40, TextsShort.twr40));
}

public class TextPackage
{
    public TextObject[] TextObjects = new TextObject[10]; 

    public TextPackage(TextObject t1, TextObject t2, TextObject t3, TextObject t4, TextObject t5, TextObject t6, 
                       TextObject t7, TextObject t8, TextObject t9, TextObject t10)
    {
        TextObjects[0] = t1;
        TextObjects[1] = t2;
        TextObjects[2] = t3;
        TextObjects[3] = t4;
        TextObjects[4] = t5;
        TextObjects[5] = t6;
        TextObjects[6] = t7;
        TextObjects[7] = t8;
        TextObjects[8] = t9;
        TextObjects[9] = t10;
    }
    public void AddTextObject(TextObject t)
    {
        TextObjects[TextObjects.Length - 1] = t;
    }
}

public class TextObject
{
    protected string TextWithErrors;
    protected string TextWithCorrections;
    protected string TextRaw;

    public TextObject(string textWErrors, string textWCorrections, string raw)
    {
        this.TextWithErrors = textWErrors;
        this.TextWithCorrections = textWCorrections;
        this.TextRaw = raw;
    }

    public string GetTextWithErrors()
    {
        return TextWithErrors;
    }

    public string GetTextWithCorrections()
    {
        return TextWithCorrections;
    }

    public string GetTextRaw()
    {
        return TextRaw;
    }
}