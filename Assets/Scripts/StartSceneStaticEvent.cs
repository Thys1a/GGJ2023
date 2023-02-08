using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFramework;
public class StartSceneStaticEvent : MonoBehaviour
{
    public VideoController video;
    private void Start()
    {
        SceneManager.Instance.PreLoadScene("1");
        video.gameObject.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// ��ʼ��Ϸ
    /// </summary>
    public void OnStartGame()
    {
        AudioController.instance.PlayOneShot("ui���");
        //Handheld.PlayFullScreenMovie(Application.streamingAssetsPath+"\"+Start.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        video.SetCallBack(() => SceneManager.Instance.LoadScene());
        video.gameObject.transform.localScale = Vector3.one;
        video.PlayVideo();
        //SceneManager.Instance.LoadScene();
    }

    /// <summary>
    /// �˳���Ϸ
    /// </summary>
    public void OnExitGame()
    {
        AudioController.instance.PlayOneShot("ui���");
        Application.Quit();
    }
}
