using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    //this is the main audio loop we want to go back to for the menu
    public AudioClip defaultTrack;
    //these two are audio sources we're going to be switching between
    private AudioSource audio1;
    private AudioSource audio2;
    private bool isPlayingAudio1;
    //for singleton class
    public static AudioManager instance;

    public AudioMixerGroup audioMixerGroup;
    public AudioMixer theMixer;

    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (PlayerPrefs.HasKey("MasterVol"))
            theMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        if (PlayerPrefs.HasKey("MusicVol"))
            theMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        if (PlayerPrefs.HasKey("SFXVol"))
            theMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
    }

    private void Start()
    {
        audio1 = gameObject.AddComponent<AudioSource>();
        audio1.loop = true;
        audio1.volume = 0.5f;
        audio1.outputAudioMixerGroup = audioMixerGroup;

        audio2 = gameObject.AddComponent<AudioSource>();
        audio2.loop = true;
        audio2.volume = 0.5f;
        audio2.outputAudioMixerGroup = audioMixerGroup;

        isPlayingAudio1 = true;
        SwapTrack(defaultTrack);
    }

    //this is what actually takes in the new audio clip to switch between
    //once AudioSwap toggles the switch
    public void SwapTrack(AudioClip newClip)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(newClip));
        isPlayingAudio1 = !isPlayingAudio1;
    }

    //for just swapping back to the main loop quickly
    public void ReturnToDefault() { SwapTrack(defaultTrack); }

    public void StopTrack()
    {
        StopAllCoroutines();
        if (isPlayingAudio1)
            audio1.Stop();
        else
            audio2.Stop();
    }
    
    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float timeToFade = 0.25f;
        float timeElapsed = 0f;
        //the bool here works to switch to the newclip, 
        // or switches back to the previous track
        if (isPlayingAudio1)
            {
                audio2.clip = newClip;
                audio2.Play();
                while (timeElapsed < timeToFade)
                    {
                     audio2.volume = Mathf.Lerp(0f, 0.5f, timeElapsed / timeToFade);
                     audio1.volume = Mathf.Lerp(0.5f, 0f, timeElapsed / timeToFade);
                     timeElapsed += Time.deltaTime;
                     yield return null;
                    }
                audio1.Stop();
                }
        else
        {
            audio1.clip = newClip;
            audio1.Play();
            while (timeElapsed < timeToFade)
                    {
                     audio1.volume = Mathf.Lerp(0f, 0.5f, timeElapsed / timeToFade);
                     audio2.volume = Mathf.Lerp(0.5f, 0f, timeElapsed / timeToFade);
                     timeElapsed += Time.deltaTime;
                     yield return null;
                    }
            audio2.Stop();
        }
        
    }
    
}
