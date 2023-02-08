using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UFramework {
    public class AudioController : MonoBehaviour
    {
        public AudioEntity BGMAudioEntity;
        private ObjectPool<AudioEntity> cacheAudioEntity;
        private Dictionary<string,AudioEntity> curAudioEntity;
        private Dictionary<Type, float> volume;
        private float volumeCoefficient;

        public static AudioController instance;

        //ȫ�ֹ㲥�¼�
        const string m_AudioPlay = "AudioPlay";
        const string m_AudioPause = "AudioPause";
        const string m_AudioStop = "AudioStop";

        public enum Type { Normal,BGM,Sound,Voice}

        #region life
        void Awake()
        {
            instance = this;
            cacheAudioEntity = new ObjectPool<AudioEntity>(AudioSourceCreate, actionOnGet, actionOnRelease, actionOnDestroy, true, 10, 20);
            curAudioEntity = new Dictionary<string, AudioEntity>();
            volume = new Dictionary<Type, float> { { Type.Normal, 1 }, { Type.BGM, 1 }, { Type.Sound, 1 }, { Type.Voice, 1 } };
            BGMAudioEntity = cacheAudioEntity.Get();

        }
        private void Start()
        {
            //DontDestroyOnLoad(this);
            //Initialize
            
        }
        #endregion

        #region CacheAudioSource Manage
        private void actionOnDestroy(AudioEntity obj)
        {
            obj.End();
        }

        private void actionOnRelease(AudioEntity obj)
        {
            obj.End();
        }

        private void actionOnGet(AudioEntity obj)
        {
            return;
        }
    
        private AudioEntity AudioSourceCreate()
        {
            AudioSource audioSource= this.gameObject.AddComponent<AudioSource>();
            AudioEntity audioEntity = new AudioEntity(cacheAudioEntity.CountAll, audioSource);
            return audioEntity;
        }
        #endregion

        #region  Playing Control
        /// <summary>
        /// ���ֲ���
        /// </summary>
        /// <param name="name">����</param>
        public void Play(string name, Type type = Type.Normal,bool isLoop=false)
        {
            AudioClip audioClip = ResourceManager.Instance.GetAudio(name);
            if (audioClip == null) return;
        
            if (curAudioEntity.ContainsKey(name))
            {
                curAudioEntity[name].Play();
            }
            else    //���������ȡһ������������ָ������
            {
                if (type == Type.BGM)//BGM���и�
                {

                    if (BGMAudioEntity.GetCurrentClipName() != null)
                    { if (curAudioEntity.ContainsKey(BGMAudioEntity.GetCurrentClipName())) curAudioEntity.Remove(BGMAudioEntity.GetCurrentClipName()); }
                    BGMAudioEntity.Load(audioClip, name,volume[type], type, true);
                    curAudioEntity.Add(name, BGMAudioEntity);

                }
                else
                {
                    AudioEntity audioEntity = cacheAudioEntity.Get();
                    audioEntity.Load(audioClip, name, volume[type], type, isLoop);
                    curAudioEntity.Add(name, audioEntity);
                
                }
            }
            MessageCenter.Instance.Send(m_AudioPlay, name);
        }
        public void PlayBGM(string name)
        {
            AudioClip audioClip = ResourceManager.Instance.GetAudio(name);
            if (audioClip == null) return;
            try
            {
                curAudioEntity.ContainsKey(name);
                curAudioEntity[name].Play();
            }
            catch    //���������ȡBGM����������ָ������
            {
                //if (BGMAudioEntity.GetCurrentClipName() != null)
                //{
                //    if (curAudioEntity.ContainsKey(BGMAudioEntity.GetCurrentClipName())) curAudioEntity.Remove(BGMAudioEntity.GetCurrentClipName());
                //}
                try
                {
                    if (curAudioEntity.ContainsValue(BGMAudioEntity)) curAudioEntity.Remove(BGMAudioEntity.GetCurrentClipName());
                }
                catch { }
                BGMAudioEntity.Load(audioClip, name, volume[Type.BGM], Type.BGM, true);
                curAudioEntity.Add(name, BGMAudioEntity);
            }
            MessageCenter.Instance.Send(m_AudioPlay,name);
        }
        public void PlayOneShot(string name)
        {
                AudioClip audioClip = ResourceManager.Instance.GetAudio(name);
                if (audioClip == null) return;
                BGMAudioEntity.audioSource.PlayOneShot(audioClip);
                MessageCenter.Instance.Send(m_AudioPlay, name);
        }

        /// <summary>
        /// ������ͣ
        /// </summary>
        /// <param name="name">����</param>
        public void Pause(string name)
        {
            if (curAudioEntity.ContainsKey(name))
            {
                curAudioEntity[name].Pause();
            }
            else
            {
                Debug.LogWarning("Pause with a wrong audio name.");
            }
            MessageCenter.Instance.Send(m_AudioPause, name);
        }

        /// <summary>
        /// ����ֹͣ
        /// </summary>
        /// <param name="name">����</param>
        public void Stop(string name,bool isCompleted=false)
        {
            if (curAudioEntity.ContainsKey(name))
            {
                curAudioEntity[name].Stop();
                if (isCompleted)
                {
                    AudioEntity audioEntity = curAudioEntity[name];
                    curAudioEntity.Remove(name);
                    if(audioEntity.GetAudioType()!=Type.BGM)cacheAudioEntity.Release(audioEntity);
                }
            
            }
            else
            {
                Debug.LogWarning("Pause with a wrong audio name.");
            }
            MessageCenter.Instance.Send(m_AudioStop, name);
        }
        #endregion

        #region VolumeControl
        public void VolumeControl(float volumeCoefficient)//����ÿ������������ȡԭ�������Ը�ϵ��
        {
            this.volumeCoefficient = volumeCoefficient;
            foreach(AudioEntity audioEntity in curAudioEntity.Values)
            {
                float newVolume = audioEntity.GetVolume();
                audioEntity.VolumeControl(newVolume * volumeCoefficient);
            }
        }
        public void VolumeControl(float newVolume, string name)//����ָ����������������ֵ
        {
            if (curAudioEntity.ContainsKey(name))
            {
                curAudioEntity[name].VolumeControl(newVolume * volumeCoefficient);
            }
            else
            {
                Debug.Log("there is no audio:" + name);
            }
        }
        public void VolumeControl(float newVolume, Type type)//����ָ����𲥷�����������ֵ
        {
            volume[type] = newVolume;
            foreach (AudioEntity audioEntity in curAudioEntity.Values)
            {
                if(audioEntity.GetAudioType()==type)audioEntity.VolumeControl(newVolume * volumeCoefficient);
            }
        }
        #endregion
    }
}