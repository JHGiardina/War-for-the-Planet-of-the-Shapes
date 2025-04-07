using TMPro;
using UnityEngine;

public class UnitsTextBehaviour : MonoBehaviour
{
    private TextMeshProUGUI tmpGUI;
    
    public void Start()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        tmpGUI.text = "Units: " + EngineScript.curPop;
    } 

}