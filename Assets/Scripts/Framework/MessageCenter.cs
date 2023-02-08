using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageCenter
{
    //单例类
    private static MessageCenter instance;
    public static MessageCenter Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MessageCenter();
            }
            return instance;
        }
    }

    //保存所有消息事件的字典
    //key使用字符串保存消息的名称
    //value使用一个带自定义参数的事件，用来调用所有注册的消息
    private Dictionary<string, EventMode> EventDictionary;
    private Stack<EventMode> waitingEvent;
    private class EventMode : UnityEvent<object>
    {

    }

    

    /// <summary>
    /// 私有构造函数
    /// </summary>
    private MessageCenter()
    {
        EventDictionary = new Dictionary<string, EventMode>();
        waitingEvent = new Stack<EventMode>();
    }

    #region public method
    /// <summary>
    /// 添加监听者
    /// </summary>
    /// <param name="key">消息名</param>
    /// <param name="action">消息事件</param>
    public void Register(string key, UnityAction<object> listener)
    {
        EventMode tempEvent = null;
        if (EventDictionary.TryGetValue(key, out tempEvent))
        {
            tempEvent.AddListener(listener);
        }
        else
        {
            tempEvent = new EventMode();
            tempEvent.AddListener(listener);
            EventDictionary.Add(key, tempEvent);
        }

    }
   

    /// <summary>
    /// 注销消息事件
    /// </summary>
    /// <param name="key">消息名</param>
    /// <param name="action">消息事件</param>
    public void Remove(string key, UnityAction<object> listener)
    {
        if (instance == null) return;
        EventMode tempEvent = null;
        if (EventDictionary.TryGetValue(key, out tempEvent))
        {
            tempEvent.RemoveListener(listener);
        }
    }


    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="key">消息名</param>

    public void Send(string key, object data)
    {
        Debug.Log(key);
        EventMode tempEvent;
        if (EventDictionary.TryGetValue(key, out tempEvent))
        {
            tempEvent.Invoke(data);
        }
    }


    /// <summary>
    /// 清空所有消息
    /// </summary>
    public void Clear()
    {
        EventDictionary.Clear();

    }
    #endregion

    private void CallBack()
    {

    }
}