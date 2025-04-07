using TMPro;
using UnityEngine;

public class HumanTextBehaviour : MonoBehaviour
{
    private TextMeshProUGUI tmpGUI;
    
    public void Start()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        tmpGUI.text = "Units: " + EngineScript.curHumanPop.ToString();
    } 

}