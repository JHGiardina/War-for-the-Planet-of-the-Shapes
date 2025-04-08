using UnityEngine;
using TMPro;
using System.Collections;

public class EngineScript : MonoBehaviour
{
    public TextMeshProUGUI RoundText;
    public WaveManager WaveManager;
    public CameraBehavior Camera;

    public int waitTimeBetweenRounds = 2;
    public static int curCount = 45; 
    public static int curPop = 0;
    public static int curHumanPop = 0;
    public static int waveNumber;

    private float curPopInt = 0f;
    private bool isWaiting = false;
    
    void Start()
    {
        waveNumber = 0;
    }

    void Update()
    {
        curHumanPop = CountHumans();
        if(curHumanPop <= 0 && isWaiting == false)
        {
            StartTransitionRound();
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

    private IEnumerator WaitRoundTextAndTransition()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeBetweenRounds);
        TransitionRound();
        isWaiting = false;
    }
}
