using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Stats : MonoBehaviour
{


    public TMPro.TextMeshProUGUI Text;
    public FloatVariable Stat;

    public void OnEnable()
    {
        Text = this.gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        if (Text == null)
        {
            Debug.LogError("No TextMesh");
        }
        else
            UpdateText();
    }

    // Could Create an abstract class
    public void UpdateText()
    {
        int stat = (int)Stat.Value;
        Text.text = stat.ToString();
    }

}
