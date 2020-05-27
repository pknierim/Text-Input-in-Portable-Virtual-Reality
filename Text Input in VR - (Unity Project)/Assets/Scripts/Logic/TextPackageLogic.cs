using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPackageLogic : MonoBehaviour
{
    public Dictionary<string, TextPackage[]> Packages = new Dictionary<string, TextPackage[]>();
    public static TextPackages TextPackages = new TextPackages();

    private static readonly TextPackage[] row1 = { TextPackages.tp1, TextPackages.tp2, TextPackages.tp3 };
    private static readonly TextPackage[] row2 = { TextPackages.tp2, TextPackages.tp3, TextPackages.tp1 };
    private static readonly TextPackage[] row3 = { TextPackages.tp3, TextPackages.tp1, TextPackages.tp2 };
    private static readonly TextPackage[] row4 = { TextPackages.tp3, TextPackages.tp2, TextPackages.tp1 };
    private static readonly TextPackage[] row5 = { TextPackages.tp1, TextPackages.tp3, TextPackages.tp2 };
    private static readonly TextPackage[] row6 = { TextPackages.tp2, TextPackages.tp1, TextPackages.tp3 };

    public void Init()
    { 

        Packages.Add("1", row1);
        Packages.Add("2", row1);
        Packages.Add("3", row1);
        Packages.Add("4", row1);
        Packages.Add("5", row1);
        Packages.Add("6", row1);
        Packages.Add("7", row2);
        Packages.Add("8", row2);
        Packages.Add("9", row2);
        Packages.Add("10", row2);
        Packages.Add("11", row2);
        Packages.Add("12", row2);
        Packages.Add("13", row3);
        Packages.Add("14", row3);
        Packages.Add("15", row3);
        Packages.Add("16", row3);
        Packages.Add("17", row3);
        Packages.Add("18", row3);
        Packages.Add("19", row4);
        Packages.Add("20", row4);
        Packages.Add("21", row4);
        Packages.Add("22", row4);
        Packages.Add("23", row4);
        Packages.Add("24", row4);
        Packages.Add("25", row5);
        Packages.Add("26", row5);
        Packages.Add("27", row5);
        Packages.Add("28", row5);
        Packages.Add("29", row5);
        Packages.Add("30", row5);
        Packages.Add("31", row6);
        Packages.Add("32", row6);
        Packages.Add("33", row6);
        Packages.Add("34", row6);
        Packages.Add("35", row6);
        Packages.Add("36", row6);
    }
} 
