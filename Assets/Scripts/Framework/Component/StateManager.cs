using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UFramework { 
public class StateManager : MonoBehaviour
{
    public static StateManager instance;
    void Awake()
    {
        instance = this;

    }
    //状态机列表
    private List<StateMachine> machines=new List<StateMachine>();
    
    private void Update()
    {
        for (int i = 0; i < machines.Count; i++)
        {
            //更新状态机
            machines[i].OnUpdate();
        }
    }
    private void OnDestroy()
    {
        instance = null;
    }

    #region Public Methods
    /// <summary>
    /// 创建状态机
    /// </summary>
    /// <typeparam name="T">状态机类型</typeparam>
    /// <param name="stateMachineName">状态机名称</param>
    /// <returns>状态机</returns>
    public T Create<T>(string stateMachineName = null) where T : StateMachine, new()
    {
        Type type = typeof(T);
        stateMachineName = string.IsNullOrEmpty(stateMachineName) ? type.Name : stateMachineName;
        if (machines.Find(m => m.Name == stateMachineName) == null)
        {
            T machine = (T)Activator.CreateInstance(type);
            machine.Name = stateMachineName;
            machines.Add(machine);
            return machine;
        }
        return default;
    }
    /// <summary>
    /// 销毁状态机
    /// </summary>
    /// <param name="stateMachineName">状态机名称</param>
    /// <returns>销毁成功返回true 否则返回false</returns>
    public bool Destroy(string stateMachineName)
    {
        var targetMachine = machines.Find(m => m.Name == stateMachineName);
        if (targetMachine != null)
        {
            targetMachine.OnDestroy();
            machines.Remove(targetMachine);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 获取状态机
    /// </summary>
    /// <typeparam name="T">状态机类型</typeparam>
    /// <param name="stateMachineName">状态机名称</param>
    /// <returns>状态机</returns>
    public T GetMachine<T>(string stateMachineName) where T : StateMachine
    {
        return (T)machines.Find(m => m.Name == stateMachineName);
    }
    #endregion
}
}