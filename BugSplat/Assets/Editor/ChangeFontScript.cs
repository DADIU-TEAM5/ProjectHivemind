using System.IO;
using UnityEngine;
using UnityEditor;
using System.Runtime.Serialization.Formatters.Binary;
namespace UnityEditor
{

    public class OpenFilePanelExample : Editor
    {

        public static TMPro.TMP_FontAsset TextFont = null;
       
        public static void Apply()
        {
           
            
         
            Debug.Log(TextFont.name);

            foreach (TMPro.TextMeshProUGUI text in FindObjectsOfType<TMPro.TextMeshProUGUI>())
            {
                Debug.Log("Changed Font");
                text.font = TextFont;

            }
        }
    }

}