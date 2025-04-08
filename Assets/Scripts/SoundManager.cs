using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // from Youtube video: https://www.youtube.com/watch?v=yWCHaTwVblk

    [SerializeField] Slider volumeSlider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetFloat("MasterVolume", 100);
            Load();
        }
        else
        {
            Load();
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        // needs the name of all the audio
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", PlayerPrefs.GetFloat("MasterVolume")); // needs the name of all the audio
    }
}
