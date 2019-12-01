using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class GameText : ScriptableObject
{
    public List<TextVariation> TextVariations;

    public LocaleVariable CurrentLocale;

    public string GetText() {

        TextVariation variation ;
        if (CurrentLocale != null) {
            variation = TextVariations.FirstOrDefault(x => x.Locale == CurrentLocale.Value);

            return variation.Text ?? "";
        }

        return "";

        //Debug.Log(variation);
        
    }

    [System.Serializable]
    public class TextVariation {
        public Locale Locale;

        [TextArea(3,8)]
        public string Text;
    }
}
