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
    //״̬���б�
    private List<StateMachine> machines=new List<StateMachine>();
    
    private void Update()
    {
        for (int i = 0; i < machines.Count; i++)
        {
            //����״̬��
            machines[i].OnUpdate();
        }
    }
    private void OnDestroy()
    {
        instance = null;
    }

    #region Public Methods
    /// <summary>
    /// ����״̬��
    /// </summary>
    /// <typeparam name="T">״̬������</typeparam>
    /// <param name="stateMachineName">״̬������</param>
    /// <returns>״̬��</returns>
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
    /// ����״̬��
    /// </summary>
    /// <param name="stateMachineName">״̬������</param>
    /// <returns>���ٳɹ�����true ���򷵻�false</returns>
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
    /// ��ȡ״̬��
    /// </summary>
    /// <typeparam name="T">״̬������</typeparam>
    /// <param name="stateMachineName">״̬������</param>
    /// <returns>״̬��</returns>
    public T GetMachine<T>(string stateMachineName) where T : StateMachine
    {
        return (T)machines.Find(m => m.Name == stateMachineName);
    }
    #endregion
}
}