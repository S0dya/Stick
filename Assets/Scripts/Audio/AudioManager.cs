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

        EventInstancesDict["Amnbience"].start();
        EventInstancesDict["MusicPiano"].start();
        EventInstancesDict["Music"].setVolume(0);
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


    public void PlayOneShot(EventReference sound, Vector3 position)
    {
        if (Settings.IsMusicEnabled)
        {
            RuntimeManager.PlayOneShot(sound, position);
        }
    }
    public void PlayOneShot(EventReference sound)
    {
        if (Settings.IsMusicEnabled)
        {
            RuntimeManager.PlayOneShot(sound, new Vector3(0, 0, 0));
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

    public void ChangeMusic(string curMusic, string newMusic)
    {
        int pos;
        EventInstancesDict[curMusic].getTimelinePosition(out pos);
        float time = pos / 1000f;

        EventInstancesDict[newMusic].setTimelinePosition((int)(time * 1000));

        fadeOutCoroutine = StartCoroutine(FadeOutMusic(EventInstancesDict[curMusic], EventInstancesDict[newMusic]));
    }
    private IEnumerator FadeOutMusic(EventInstance music, EventInstance musicFadeIn)
    {
        musicFadeIn.setVolume(0);
        musicFadeIn.start();

        float timer = 0.5f;
        float timerFadeIn = 0f;


        while (timer > 0)
        {
            music.setVolume(timer * 1.5f);
            musicFadeIn.setVolume(timerFadeIn * 1.5f);
            Debug.Log(timer * 1.5f);
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

    public void ToggleMusic(bool val)
    {
        if (val)
        {
            EventInstancesDict["MusicPiano"].start();
            EventInstancesDict["FliesAmbience"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        else
        {
            EventInstancesDict["MusicPiano"].start();
            EventInstancesDict["FliesAmbience"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            EventInstancesDict["Music"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void PlayInstance(string instanceName)
    {
        EventInstancesDict[instanceName].start();
    }
}