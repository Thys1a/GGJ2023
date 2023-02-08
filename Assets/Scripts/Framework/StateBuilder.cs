using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework
{
    /// <summary>
    /// ״̬������������״̬����
    /// </summary>
    /// <typeparam name="T">״̬����</typeparam>
    public class StateBuilder<T> where T : State, new()
    {
        //������״̬
        private readonly T state;
        //������״̬������״̬��
        private readonly StateMachine stateMachine;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="state"></param>
        /// <param name="stateMachine"></param>
        public StateBuilder(T state, StateMachine stateMachine)
        {
            this.state = state;
            this.stateMachine = stateMachine;
        }

        /// <summary>
        /// ����״̬��ʼ���¼�
        /// </summary>
        /// <param name="onInitialization">״̬��ʼ���¼�</param>
        /// <returns>״̬������</returns>
        public StateBuilder<T> OnInitialization(Action<T> onInitialization)
        {
            state.onInitialization = () => onInitialization(state);
            return this;
        }
        /// <summary>
        /// ����״̬�����¼�
        /// </summary>
        /// <param name="onEnter">״̬�����¼�</param>
        /// <returns>״̬������</returns>
        public StateBuilder<T> OnEnter(Action<T> onEnter)
        {
            state.onEnter = () => onEnter(state);
            return this;
        }
        /// <summary>
        /// ����״̬ͣ���¼�
        /// </summary>
        /// <param name="onStay">״̬ͣ���¼�</param>
        /// <returns>״̬������</returns>
        public StateBuilder<T> OnStay(Action<T> onStay)
        {
            state.onStay = () => onStay(state);
            return this;
        }
        /// <summary>
        /// ����״̬�˳��¼�
        /// </summary>
        /// <param name="onExit">״̬�˳��¼�</param>
        /// <returns>״̬������</returns>
        public StateBuilder<T> OnExit(Action<T> onExit)
        {
            state.onExit = () => onExit(state);
            return this;
        }
        /// <summary>
        /// ����״̬��ֹ�¼�
        /// </summary>
        /// <param name="onTermination">״̬��ֹ�¼�</param>
        /// <returns>״̬������</returns>
        public StateBuilder<T> OnTermination(Action<T> onTermination)
        {
            state.onTermination = () => onTermination(state);
            return this;
        }
        /// <summary>
        /// ����״̬�л�����
        /// </summary>
        /// <param name="predicate">�л�����</param>
        /// <param name="targetStateName">Ŀ��״̬����</param>
        /// <returns>״̬������</returns>
        public StateBuilder<T> SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            state.SwitchWhen(predicate, targetStateName);
            return this;
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <returns>״̬��</returns>
        public StateMachine Complete()
        {
            state.OnInitialization();
            return stateMachine;
        }


    }
}
