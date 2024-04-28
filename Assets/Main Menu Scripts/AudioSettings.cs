using UnityEngine;

public class AudioSettings : MonoBehaviour
{

    private static readonly string musicPref = "musicPref";
    private static readonly string sfxPref = "sfxPref";

    private float musicFloat, sfxFloat;

    public AudioSource musicAudio;
    public AudioSource[] sfxAudio;

    void Awake()
    {
        ContinueSettings();
    }

    private void ContinueSettings()
    {
        musicFloat = PlayerPrefs.GetFloat(musicPref);
        sfxFloat = PlayerPrefs.GetFloat(sfxPref);

        musicAudio.volume = musicFloat;

        for (int i = 0; i < sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxFloat;
        }
    }
}
