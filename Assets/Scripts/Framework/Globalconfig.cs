using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework
{
    [CreateAssetMenu(menuName = "Config ",fileName = "GlobalConfig")]
    public class Globalconfig : ScriptableObject
    {
        public string audioPath;
        public string uiprefabPath;
        public string backgroundPath;
        public string spritePath;
    }
}

