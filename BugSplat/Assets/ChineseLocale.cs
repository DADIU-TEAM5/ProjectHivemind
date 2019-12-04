using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChineseLocale : MonoBehaviour
{
    public TMPro.TMP_FontAsset ChineseFont;
    public LocaleVariable CurrentLocale;
    public Locale Chinese;
    public void ChangeToChinese(TMPro.TextMeshProUGUI textMesh)
    {
        if (CurrentLocale.Value == Chinese)
        {
            textMesh.font = ChineseFont;
        }
    }


}
