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
        // 清除Raw Image的残留帧
        //videoPlayer.targetTexture.Release();
        // 监听视频播放结束
        videoPlayer.loopPointReached += EndReached;
        skipBtn.onClick.AddListener(OnSkipBtnClick);
    }

    private void EndReached(VideoPlayer source)
    {
        if (callback != null) callback.Invoke();
        // 隐藏当前脚本对象
        gameObject.SetActive(false);
    }

    // 外部调用播放
    public void PlayVideo(string url=null)
    {
        if(url!=null)videoPlayer.url = Application.streamingAssetsPath + "/" + url+".mp4";
        videoPlayer.Play();
    }
    public void SetCallBack(Action action)
    {
        this.callback = action;
    }

    // 跳过视频
    private void OnSkipBtnClick()
    {
        videoPlayer.Stop();
        EndReached(videoPlayer);
    }


}
