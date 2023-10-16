using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FPS/Footstep Audio Data")]
public class FootStepAudioData : ScriptableObject
{
    public List<FootStepAudio> footStepAudios = new List<FootStepAudio>();
}


[System.Serializable]
public class FootStepAudio
{
    public string tag;
    public List<AudioClip> audioClips = new List<AudioClip>();
    public float delay;
}