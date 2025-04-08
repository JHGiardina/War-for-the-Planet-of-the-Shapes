using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    public static MainMenuAudio instance;
    public static AudioSource Audio;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Audio = GetComponent<AudioSource>();
        }

        if(!MainMenuAudio.Audio.isPlaying)
        {
            Audio.Play();
        }
    }
}