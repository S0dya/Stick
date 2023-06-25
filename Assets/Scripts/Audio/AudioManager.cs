using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : SingletonMonobehaviour<AudioManager>
{
    List<EventInstance> eventInstances;
    List<StudioEventEmitter> eventEmitters;

    Dictionary<string, EventInstance> EventInstancesDict;

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
        EventInstancesDict.Add("Ambience", CreateInstance(FMODManager.Instance.Amnbience));
        EventInstancesDict.Add("Music", CreateInstance(FMODManager.Instance.Music));

        EventInstancesDict.Add("ButtonPress", CreateInstance(FMODManager.Instance.ButtonPress));
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

    /*
    void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    void OnDestroy() //maybe delete later
    {
        CleanUp();
    }
    */

    public void PlayInstance(string instanceName)
    {
        EventInstancesDict[instanceName].start();
    }



        /*
        if (GameManager.enemiesFollowingPlayer == 0)
        {
            fightMusicInstance.stop();
            calmMusicInstance.start();
        }
        else
        {
            if (GameManager.enemiesFollowingPlayer == 1)
            {
                fightMusicInstance.start();
                calmMusicInstance.stop();
            }
        }*/
}