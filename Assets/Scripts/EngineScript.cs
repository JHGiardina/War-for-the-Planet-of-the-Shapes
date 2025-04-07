using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class EngineScript : MonoBehaviour
{
    public TextMeshProUGUI RoundText;
    public WaveManager WaveManager;
    public CameraBehavior Camera;


    public static int curCount = 45; 
    public static int curPop = 0;
    private float curPopInt = 0f;
    public static int curRound = 0;

    private int waveNumber;
    private bool isWaiting = false;
    [Header("Settings ")]
    public int waitTimeBetweenRounds = 2;
    public int finalRoundNumber = 10;
    
    void Start()
    {
        waveNumber = 0;
    }

    void Update()
    {
        if (curRound > finalRoundNumber)
        {
            SceneManager.LoadScene("VictoryScene");
        }
        if(CountHumans() <= 0 && isWaiting == false)
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
        int count = GameObject.FindGameObjectsWithTag("Prism").Length;
        return count;
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
