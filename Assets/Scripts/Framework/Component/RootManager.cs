using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootManager: MonoBehaviour
{
    public const string rootPath = "RootCanvas";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    
    public static void Init()
    {
        GameObject obj = Instantiate((GameObject)Resources.Load(rootPath));
        DontDestroyOnLoad(obj);
    }
}
