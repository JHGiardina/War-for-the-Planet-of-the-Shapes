using UnityEngine;
using TMPro;
using System.Collections;

public class EngineScript : MonoBehaviour
{
    public TextMeshProUGUI RoundText;
    public WaveManager WaveManager;
    public CameraBehavior Camera;

    public int waitTimeBetweenRounds = 2;
    public int passiveResourceAmount = 10;
    public int passiveResourceRate = 5;

    [HideInInspector] public static int curCount = 45; 
    [HideInInspector] public static int curPop = 0;
    [HideInInspector] public static int curHumanPop = 0;
    [HideInInspector] public static int waveNumber;

    private bool isWaiting = false;
    private float timeLastPassiveResource;
    
    void Start()
    {
        timeLastPassiveResource = Time.time;
        waveNumber = 0;
    }

    void Update()
    {
        curHumanPop = CountHumans();
        curPop = CountPrisms();
        
        if(curHumanPop <= 0 && isWaiting == false)
        {
            StartTransitionRound();
        } 
        else
        {
            AddPassiveResources();
        }
    }

    private int CountPrisms()
    {
        PrismUnitBehaviour[] prisms = GameObject.FindObjectsByType<PrismUnitBehaviour>(FindObjectsSortMode.None);

        // minus 1 b/c presumably there is a base which is a prism unit
        int prismCount = prisms.Length - 1;
        return prismCount;
    }

    private void StartTransitionRound()
    {
        // User loses control during round transitions 
        Camera.IsUserControllable = false;

        Camera.CameraToRoundTextPosition();

        waveNumber++;

        RoundText.enabled = true;
        RoundText.text = "Round " + waveNumber;
        StartCoroutine(WaitRoundTextAndTransition());
    }    

    private void TransitionRound()
    {

        Camera.ReturnCameraToPreviousPosition();
        WaveManager.SpawnWave();
        
        RoundText.enabled = false;

        // User regains control (must also wait for transition to finish)
        Camera.IsUserControllable = true;
    }

    private int CountHumans()
    {
        BaseHumanUnitBehaviour[] aliveHumans = GameObject.FindObjectsByType<BaseHumanUnitBehaviour>(FindObjectsSortMode.None);
        return aliveHumans.Length;
    }

    private void AddPassiveResources()
    {
        float timeSinceLastPassiveResource = Time.time - timeLastPassiveResource;
        if(timeSinceLastPassiveResource >= passiveResourceRate)
        {
            timeLastPassiveResource = Time.time;
            curCount += passiveResourceAmount;
        }
    }

    private IEnumerator WaitRoundTextAndTransition()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeBetweenRounds);
        TransitionRound();
        isWaiting = false;
    }
}
