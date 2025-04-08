using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// Code taken from KingP menu behavior
public class MenuBehavior : MonoBehaviour
{
    public void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleMap");
    }

    public void GoToMenu()
    {
        Debug.Log("menu");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void GoToSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SettingsMenu");
    }

    public void GoToDiffSelect()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DifficultySelect");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
