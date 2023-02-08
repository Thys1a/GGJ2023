using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework
{

    public class ResourceManager
    {
        private static ResourceManager instance;
        private Globalconfig globalconfig;

        //全局广播事件
        const string m_ConfigLoaded = "ConfigLoaded";

        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResourceManager();
                }
                return instance;
            }
        }
        ResourceManager()
        {
            globalconfig = Resources.Load(configPath) as Globalconfig;
            MessageCenter.Instance.Send(m_ConfigLoaded,"GlobalConfig");
        }
        public Globalconfig GetGlobalconfig { get { return globalconfig; } }

        private const string configPath = "GlobalConfig";

        public  AudioClip GetAudio(string name)
        {
            AudioClip clip = null;
            try
            {
                clip = (AudioClip)Resources.Load(globalconfig.audioPath + name);
            }
            catch (System.Exception)
            {
                Debug.LogWarning("GetAudio:didn't get the wanted audio.");
            }
            return clip;
        }
    }
}