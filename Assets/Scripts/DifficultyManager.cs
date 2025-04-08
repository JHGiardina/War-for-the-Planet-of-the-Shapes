using UnityEngine;
using TMPro;

public enum Difficulty {Easy, Medium, Hard}

public class DifficultyManager : MonoBehaviour
{

    public static DifficultyManager instance;
    public static Difficulty DifficultyLevel;

    public TextMeshProUGUI difficultyText;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            DifficultyManager.DifficultyLevel = Difficulty.Easy;
        }
    }

    private void Update()
    {
        UpdateDifficultyText();
    }

    public void SetEasy()
    {
        NewDifficultySelected(Difficulty.Easy);
    }

    public void SetMedium()
    {
        NewDifficultySelected(Difficulty.Medium);
    }

    public void SetHard()
    {
        NewDifficultySelected(Difficulty.Hard);
    }

    private void NewDifficultySelected(Difficulty diflvl)
    {
        DifficultyManager.DifficultyLevel = diflvl;
        Debug.Log("Difficulty selected: " + diflvl);
    }

    private void UpdateDifficultyText()
    {
        difficultyText.text = "Difficulty: " + DifficultyManager.DifficultyLevel.ToString();
    }
}
