using UnityEngine;

public class EngineScript : MonoBehaviour
{
    public static int curCount = 45; 
    public static int curPop = 0;
    private float curPopInt = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        curPopInt += Time.deltaTime;
        if (curPopInt >= 5)
        {
            curPop = CountPrisms();
            curPopInt = 0;
        }
    }

    private int CountPrisms()
    {
        int count = GameObject.FindGameObjectsWithTag("Prism").Length;
        return count;
        
    }
}
