using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class LocaleChooser : MonoBehaviour
{
    public LocaleManager Manager;

    public Locale[] Locales;

    public LocaleVariable CurrentLocale;

    public GameText[] gameTexts;

    public List<TMPro.TextMeshPro> AllTM;

    private int index = 0;

    void Start() {
        index = Array.IndexOf(Locales, CurrentLocale.Value);
        TextMeshSetup();
    }

    public void NextLocale() {
        // Update the index
        index++;
        index %= Locales.Length;        

        // Pick next locale
        Manager.SetLocale(Locales[index]);
    }
    
    void TextMeshSetup()
    {

    }

}
