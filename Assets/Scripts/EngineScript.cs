using UnityEngine;
using TMPro;

public class EngineScript : MonoBehaviour
{
    public TextMeshProUGUI RoundText;
    public WaveManager WaveManager;
    public CameraBehavior Camera;

    public static int curCount = 45; 
    public static int curPop = 0;
    private float curPopInt = 0f;

    private int waveNumber;
    
    void Start()
    {
        waveNumber = 0;
    }

    void Update()
    {
        if(GetNumberOfHumans() <= 0)
        {
            TransitionRound();
        }
        
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

    private void TransitionRound()
    {
        waveNumber++;
        RoundText.text = "Round " + waveNumber;
        WaveManager.SpawnWave();
    }

    private int GetNumberOfHumans()
    {
        BaseHumanUnitBehaviour[] aliveHumans = GameObject.FindObjectsByType<BaseHumanUnitBehaviour>(FindObjectsSortMode.None);
        return aliveHumans.Length;
    }
}
