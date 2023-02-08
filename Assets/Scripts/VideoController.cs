using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Button skipBtn;
    private Action callback;
    private void Start()
    {
        // ���Raw Image�Ĳ���֡
        //videoPlayer.targetTexture.Release();
        // ������Ƶ���Ž���
        videoPlayer.loopPointReached += EndReached;
        skipBtn.onClick.AddListener(OnSkipBtnClick);
    }

    private void EndReached(VideoPlayer source)
    {
        if (callback != null) callback.Invoke();
        // ���ص�ǰ�ű�����
        gameObject.SetActive(false);
    }

    // �ⲿ���ò���
    public void PlayVideo(string url=null)
    {
        if(url!=null)videoPlayer.url = Application.streamingAssetsPath + "/" + url+".mp4";
        videoPlayer.Play();
    }
    public void SetCallBack(Action action)
    {
        this.callback = action;
    }

    // ������Ƶ
    private void OnSkipBtnClick()
    {
        videoPlayer.Stop();
        EndReached(videoPlayer);
    }


}
