using UnityEngine;
using TMPro;

public enum Difficulty {Easy, Medium, Hard}

public class DifficultyManager : MonoBehaviour
{

    public static DifficultyManager _instance;

    public static DifficultyManager GetInstance()
    {
        return _instance;
    }

    public Difficulty DifficultyLevel = Difficulty.Easy;
    public TextMeshProUGUI difficultyText;

    void Awake()
    {
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        UpdateDifficultyText();
        ApplyDifficulty(DifficultyLevel);
    }

    public void NewDifficultySelected(Difficulty diflvl)
    {
        DifficultyLevel = diflvl;
        ApplyDifficulty(diflvl);
        UpdateDifficultyText();
        Debug.Log("Difficulty selected: " + diflvl);
    }

    public void ApplyDifficulty(Difficulty level)
    {
        // game behavior here
        Debug.Log("Applying difficulty: " + level);
    }

    public void UpdateDifficultyText()
    {
        if (difficultyText != null)
        {
            difficultyText.text = "Difficulty: " + DifficultyLevel.ToString();
        }
    }
}
