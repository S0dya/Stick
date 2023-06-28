using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODManager : SingletonMonobehaviour<FMODManager>
{
    [field: Header("Ambience")]

    [field: SerializeField] public EventReference Amnbience { get; private set; }
    [field: SerializeField] public EventReference FliesAmnbience { get; private set; }

    [field: Header("Music")]

    [field: SerializeField] public EventReference Music { get; private set; }
    [field: SerializeField] public EventReference MusicPiano { get; private set; }

    [field: Header("Player")]

    [field: SerializeField] public EventReference TongueLengthen { get; private set; }
    [field: SerializeField] public EventReference TongueShorten { get; private set; }

    [field: Header("Enemy")]

    [field: SerializeField] public EventReference FlySound { get; private set; }
    [field: SerializeField] public EventReference BeeSound { get; private set; }
    [field: SerializeField] public EventReference FireFlySound { get; private set; }

    [field: Header("UI")]

    [field: SerializeField] public EventReference ButtonPress { get; private set; }

    [field: SerializeField] public EventReference[] CatchSounds { get; private set; }

    [field: SerializeField] public EventReference PlaySound { get; private set; }
    [field: SerializeField] public EventReference GameOverSound { get; private set; }

    protected override void Awake()
    {
        base.Awake();

    }
}