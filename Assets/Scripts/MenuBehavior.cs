using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// Code taken from KingP menu behavior
public class MenuBehavior : MonoBehaviour
{
    public void GoToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleMap");
        //StartCoroutine(WaitForSoundAndTransition("BattleMap"));
    }

    public void GoToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        //StartCoroutine(WaitForSoundAndTransition("MainMenu"));
    }

    public void GoToSettings()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SettingsMenu");
        //StartCoroutine(WaitForSoundAndTransition("SettingsMenu"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
