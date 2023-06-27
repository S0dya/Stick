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

        EventInstancesDict["Amnbience"].start();
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
        RuntimeManager.PlayOneShot(sound, position);
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
            sound.Value.setParameterByName("volume", volume);
        }
    }

    public void ChangeMusic(string curMusic, string newMusic)
    {
        int pos;
        EventInstancesDict[curMusic].getTimelinePosition(out pos);
        float time = pos / 1000f;

        EventInstancesDict[newMusic].setTimelinePosition((int)(time * 1000));
        EventInstancesDict[curMusic].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        EventInstancesDict[newMusic].start();
    }

    public void ToggleMusic(bool val)
    {
        if (val)
        {
            EventInstancesDict["Music"].start();
        }
        else
        {
            EventInstancesDict["Music"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            EventInstancesDict["MusicPiano"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
        
    }

    public void PlayInstance(string instanceName)
    {
        EventInstancesDict[instanceName].start();
    }
}