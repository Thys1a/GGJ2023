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
        AudioController.instance.Stop("��һ�����ʱ�ӳ���");
        SceneManager.Instance.LoadScene();
    }
    public void StartEvent()
    {
        AudioController.instance.Play("��һ�����ʱ�ӳ���");
    }
}
