using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    [HideInInspector] public Dictionary<string, EventInstance> EventInstancesDict;

    bool calmMusicIsCurrentlyPlaying;

    Coroutine fadeOutCoroutine;

    protected override void Awake()
    {
        base.Awake();

        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();
        EventInstancesDict = new Dictionary<string, EventInstance>();

    }

    void Start()
    {
        EventInstancesDict.Add("Music", CreateInstance(FMODManager.Instance.Music));
        EventInstancesDict.Add("MusicPiano", CreateInstance(FMODManager.Instance.MusicPiano));
        EventInstancesDict.Add("ButtonPress", CreateInstance(FMODManager.Instance.ButtonPress));
        EventInstancesDict.Add("Amnbience", CreateInstance(FMODManager.Instance.Amnbience));
        EventInstancesDict.Add("FliesAmbience", CreateInstance(FMODManager.Instance.FliesAmnbience));
        EventInstancesDict.Add("PlaySound", CreateInstance(FMODManager.Instance.PlaySound));
        EventInstancesDict.Add("GameOverSound", CreateInstance(FMODManager.Instance.GameOverSound));
        EventInstancesDict.Add("FlySound", CreateInstance(FMODManager.Instance.FlySound));
        EventInstancesDict.Add("BeeSound", CreateInstance(FMODManager.Instance.BeeSound));
        EventInstancesDict.Add("FireFlySound", CreateInstance(FMODManager.Instance.FireFlySound));
        for (int i = 0; i < FMODManager.Instance.CatchSounds.Length; i++)
        {
            EventInstancesDict.Add("CatchSounds" + i, CreateInstance(FMODManager.Instance.CatchSounds[i]));
        }

        EventInstancesDict["Amnbience"].start();
        EventInstancesDict["MusicPiano"].start();
    }


    public void SetParameter(string instanceName, string parameterName, float value)
    {
        EventInstancesDict[instanceName].setParameterByName(parameterName, value);
    }
    public void SetParameterWithCheck(string instanceName, string parameterName, float newValue)
    {
        float currentParameterValue;
        EventInstancesDict[parameterName].getParameterByName(parameterName, out currentParameterValue);

        if (currentParameterValue != newValue)
        {
            EventInstancesDict[parameterName].setParameterByName(parameterName, newValue);
        }
    }


    public void PlayOneShot(string sound)
    {
        if (Settings.IsMusicEnabled)
        {
            EventInstancesDict[sound].start();
        }
    }

    public EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(eventInstance);

        return eventInstance;
    }


    public StudioEventEmitter initializeEventEmitter(EventReference eventReference, GameObject emitterGameO)
    {
        StudioEventEmitter emitter = emitterGameO.GetComponent<StudioEventEmitter>();

        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);

        return emitter;
    }

    public void ToggleSound(bool val)
    {
        float volume = val ? 1f : 0f;
        foreach(var sound in EventInstancesDict)
        {
            sound.Value.setVolume(volume);
        }
    }

    public void ChangeMusic()
    {
        string curMusic = "MusicPiano";
        string newMusic = "Music";
        PLAYBACK_STATE playbackState;
        EventInstancesDict["Music"].getPlaybackState(out playbackState);
        if (playbackState == PLAYBACK_STATE.PLAYING)
        {
            Debug.Log("D");
            curMusic = "Music";
            newMusic = "MusicPiano";
        }
        
        int pos;
        EventInstancesDict[curMusic].getTimelinePosition(out pos);
        float time = pos / 1000f;

        EventInstancesDict[newMusic].setTimelinePosition((int)(time * 1000));

        if (!Settings.IsMusicEnabled)
        {
            EventInstancesDict[curMusic].start();
            EventInstancesDict[newMusic].stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
        else
        {
            fadeOutCoroutine = StartCoroutine(FadeOutMusic(EventInstancesDict[curMusic], EventInstancesDict[newMusic]));
        }
    }
    private IEnumerator FadeOutMusic(EventInstance music, EventInstance musicFadeIn)
    {
        musicFadeIn.setVolume(0);
        musicFadeIn.start();

        float timer = 0.25f;
        float timerFadeIn = 0f;

        while (timer > 0)
        {
            music.setVolume(timer * 4f);
            musicFadeIn.setVolume(timerFadeIn * 4f);
            timer -= Time.deltaTime;
            timerFadeIn += Time.deltaTime;

            if (!Settings.IsMusicEnabled)
            {
                music.setVolume(0);
                musicFadeIn.setVolume(0);
                break;
            }

            yield return null;
        }

        music.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void PlayInstance(string instanceName)
    {
        EventInstancesDict[instanceName].start();
    }
}