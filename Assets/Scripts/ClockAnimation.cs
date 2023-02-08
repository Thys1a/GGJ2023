using System.Collections;
using System.Collections.Generic;
using UFramework;
using UnityEngine;

public class ClockAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallBack()
    {
        AudioController.instance.Stop("第一关最后时钟出现");
        SceneManager.Instance.LoadScene();
    }
    public void StartEvent()
    {
        AudioController.instance.Play("第一关最后时钟出现");
    }
}
