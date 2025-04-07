using TMPro;
using UnityEngine;

public class ResourceTextBehaviour : MonoBehaviour
{
    private TextMeshProUGUI tmpGUI;
    
    public void Start()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
    }

    public void Update()
    {
        tmpGUI.text = "Resources: " + EngineScript.curCount.ToString();
    } 

}