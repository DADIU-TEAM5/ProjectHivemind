using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateLevelUI : MonoBehaviour
{
    public IntVariable CurrentLevelSO;
    public TextMeshProUGUI LevelUI;
    
    // Start is called before the first frame update
    void Start()
    {
        LevelUI.text = CurrentLevelSO.Value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
