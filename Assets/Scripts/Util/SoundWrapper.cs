using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Enums;

[Serializable]
public class SoundWrapper
{
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private SoundType soundType;

    public AudioClip AudioClip => audioClip;
    public SoundType SoundType => soundType;
}