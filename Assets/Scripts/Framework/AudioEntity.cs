using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework
{ 
public class AudioEntity 
{
    public int id;
    public float volume=0;
    public string currentClipName;
    public AudioController.Type type;
    public AudioSource audioSource;

    private bool pause;
  
    #region public functions of playing control
    /// <summary>
    /// º”‘ÿ“Ù¿÷∆¨∂Œ
    /// </summary>
    public void Load(AudioClip clip,string clipName, float volume,AudioController.Type type= AudioController.Type.Normal,bool isLoop=false)
    {
        Init();


        audioSource.loop = isLoop;
        audioSource.clip = clip;
        currentClipName = clipName;
        VolumeControl(volume);
        this.type = type;
        Play();
    }

    public void Play()
    {
        if (currentClipName == null)
        {
            Debug.Log("there is no clip in this AudioSource" + id);
            return;
        }
        else if (pause == true) {
            audioSource.UnPause();
            pause = false;
        }
        else audioSource.Play();
    }
    public void Pause()
    {
        if (currentClipName == null)
        {
            Debug.Log("there is no clip in this AudioSource" + id);
            return;
        }
        else audioSource.Pause();
        pause = true;
    }
    public void Stop()
    {
        if (currentClipName == null)
        {
            Debug.Log("there is no clip in this AudioSource" + id);
            return;
        }
        else audioSource.Stop();
        
    }

    public void End()
    {
        Stop();
        Init();
    }

    public void VolumeControl(float volume)
    {
        audioSource.volume = volume;
    }
    public void Mute(bool mute = true)
    {
        if (mute)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
    }
    #endregion

    public AudioEntity(int id,AudioSource audioSource)
    {
        this.id = id;
        this.audioSource = audioSource;
        pause = false;
    }
    public string GetCurrentClipName()
    {
        return currentClipName;
    }
    public AudioController.Type GetAudioType() { return type; }
    public float GetVolume() { return audioSource.volume; }

    private void Init()
    {
        audioSource.clip = null;
        currentClipName = null;
        volume = 0;
        pause = false;
        Mute(false);
    }


}
}
