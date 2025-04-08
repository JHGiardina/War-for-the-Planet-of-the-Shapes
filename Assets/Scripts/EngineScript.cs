using UnityEngine;
using TMPro;
using System.Collections;

public class EngineScript : MonoBehaviour
{
    public TextMeshProUGUI RoundText;
    public WaveManager WaveManager;
    public CameraBehavior Camera;
    public CollectionSpawning collectionSpawner; 

    public int waitTimeBetweenRounds = 2;
    public int passiveResourceAmount = 10;
    public int passiveResourceRate = 5;

    [HideInInspector] public static int curCount = 45; 
    [HideInInspector] public static int curPop = 0;
    [HideInInspector] public static int curHumanPop = 0;
    [HideInInspector] public static int waveNumber;

    private bool isWaiting;
    private bool isVictory;
    private float timeLastPassiveResource;
    private Difficulty difficulty; 
    private int maxRounds;
    private AudioSource victorySound;
    
    void Start()
    {
        // Get/Change state of Singleton Classes
        if(MainMenuAudio.Audio != null)
        {
            MainMenuAudio.Audio.Stop();
        }

        // Get Difficulty from persistent singleton
        SetDifficulty();

        Debug.Log(difficulty);
        
        victorySound = GetComponent<AudioSource>();
        timeLastPassiveResource = Time.time;
        waveNumber = 0;
        curCount = 45;
        curPop = 0;
        curHumanPop = 0;
        isVictory = false;
        isWaiting = false;
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
        Debug.Log("transition");

        // Check if the user won the game
        if(waveNumber >= maxRounds && !isVictory)
        {
            StartCoroutine(WaitForSoundAndTransition("VictoryScene"));
            return;
        }

        // User loses control during round transitions 
        Camera.IsUserControllable = false;
        Camera.CameraToRoundTextPosition();
        Debug.Log("Wave number" + waveNumber);
        RoundText.enabled = true;
        waveNumber++;
        RoundText.text = "Round " + waveNumber;
        StartCoroutine(WaitRoundTextAndTransition());
    }    

    private void TransitionRound()
    {

        Camera.ReturnCameraToPreviousPosition();
        WaveManager.SpawnWave();
        collectionSpawner.ResetCollectors();

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

    private void SetDifficulty()
    {
        // Get data from singleton difficulty manager
        if(DifficultyManager.DifficultyLevel == null)
        {
            difficulty = Difficulty.Easy;
        }
        else
        {
            difficulty = DifficultyManager.DifficultyLevel;
        }

        switch(difficulty)
        {
            case Difficulty.Easy:
                maxRounds = 1;
                break;
            case Difficulty.Medium:
                maxRounds = 10;
                break;
            case Difficulty.Hard:
                maxRounds = 15;
                break;
        }
        Debug.Log("Max Rounds " + maxRounds);
    }

    private IEnumerator WaitRoundTextAndTransition()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTimeBetweenRounds);
        TransitionRound();
        isWaiting = false;
    }

    // Derived from king pin project
    private IEnumerator WaitForSoundAndTransition(string sceneName)
    {
        isVictory = true;
        victorySound.Play();
        Debug.Log("victorySoundPlay");
        yield return new WaitForSeconds(victorySound.clip.length);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
