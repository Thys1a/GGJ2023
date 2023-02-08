using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework
{
    /// <summary>
    /// ״̬�ӿ�
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// ״̬����
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// ״̬��ʼ���¼�
        /// </summary>
        void OnInitialization();
        /// <summary>
        /// ״̬�����¼�
        /// </summary>
        void OnEnter();
        /// <summary>
        /// ״̬ͣ���¼���Update��
        /// </summary>
        void OnStay();
        /// <summary>
        /// ״̬�˳��¼�
        /// </summary>
        void OnExit();
        /// <summary>
        /// ״̬��ֹ�¼�
        /// </summary>
        void OnTermination();
        /// <summary>
        /// ״̬�л�����
        /// </summary>
        /// <param name="predicate">�л�����</param>
        /// <param name="targetStateName">Ŀ��״̬����</param>
        void SwitchWhen(Func<bool> predicate, string targetStateName);

    }
}
