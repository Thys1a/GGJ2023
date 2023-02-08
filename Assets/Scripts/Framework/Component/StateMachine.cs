using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework {
    public class StateMachine : MonoBehaviour
    {
        //״̬�б� �洢״̬��������״̬
        private readonly List<IState> states = new List<IState>();
        //״̬�л������б�
        private List<StateSwitchCondition> conditions = new List<StateSwitchCondition>();

        /// <summary>
        /// ״̬������
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ��ǰ״̬
        /// </summary>
        public IState CurrentState { get; protected set; }

        /// <summary>
        /// ���״̬
        /// </summary>
        /// <param name="state">״̬</param>
        /// <returns>��ӳɹ�����true ���򷵻�false</returns>
        public bool Add(IState state)
        {
            //�ж��Ƿ��Ѿ�����
            if (!states.Contains(state))
            {
                //�ж��Ƿ����ͬ��״̬
                if (states.Find(m => m.Name == state.Name) == null)
                {
                    //�洢���б�
                    states.Add(state);
                    //ִ��״̬��ʼ���¼�
                    state.OnInitialization();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// ���״̬
        /// </summary>
        /// <typeparam name="T">״̬����</typeparam>
        /// <param name="stateName">״̬����</param>
        /// <returns>��ӳɹ�����true ���򷵻�false</returns>
        public bool Add<T>(string stateName = null) where T : IState, new()
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);
            t.Name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
            return Add(t);
        }
        /// <summary>
        /// �Ƴ�״̬
        /// </summary>
        /// <param name="state">״̬</param>
        /// <returns>�Ƴ��ɹ�����true ���򷵻�false</returns>
        public bool Remove(IState state)
        {
            //�ж��Ƿ����
            if (states.Contains(state))
            {
                //���Ҫ�Ƴ���״̬Ϊ��ǰ״̬ ����ִ�е�ǰ״̬�˳��¼�
                if (CurrentState == state)
                {
                    CurrentState.OnExit();
                    CurrentState = null;
                }
                //ִ��״̬��ֹ�¼�
                state.OnTermination();
                return states.Remove(state);
            }
            return false;
        }
        /// <summary>
        /// �Ƴ�״̬
        /// </summary>
        /// <param name="stateName">״̬����</param>
        /// <returns>�Ƴ��ɹ�����true ���򷵻�false</returns>
        public bool Remove(string stateName)
        {
            var targetIndex = states.FindIndex(m => m.Name == stateName);
            if (targetIndex != -1)
            {
                var targetState = states[targetIndex];
                if (CurrentState == targetState)
                {
                    CurrentState.OnExit();
                    CurrentState = null;
                }
                targetState.OnTermination();
                return states.Remove(targetState);
            }
            return false;
        }
        /// <summary>
        /// �Ƴ�״̬
        /// </summary>
        /// <typeparam name="T">״̬����</typeparam>
        /// <returns>�Ƴ��ɷ���true ���򷵻�false</returns>
        public bool Remove<T>() where T : IState
        {
            return Remove(typeof(T).Name);
        }
        /// <summary>
        /// �л�״̬
        /// </summary>
        /// <param name="state">״̬</param>
        /// <returns>�л��ɹ�����true ���򷵻�false</returns>
        public bool Switch(IState state)
        {
            //�����ǰ״̬�Ѿ����л���Ŀ��״̬ �����л� ����false
            if (CurrentState == state) return false;
            //��ǰ״̬��Ϊ����ִ��״̬�˳��¼�
            CurrentState?.OnExit();
            //�ж��л���Ŀ��״̬�Ƿ�������б���
            if (!states.Contains(state)) return false;
            //���µ�ǰ״̬
            CurrentState = state;
            //���º� ��ǰ״̬��Ϊ����ִ��״̬�����¼�
            CurrentState?.OnEnter();
            return true;
        }
        /// <summary>
        /// �л�״̬
        /// </summary>
        /// <param name="stateName">״̬����</param>
        /// <returns>�л��ɹ�����true ���򷵻�false</returns>
        public bool Switch(string stateName)
        {
            //����״̬�������б��в�ѯ
            var targetState = states.Find(m => m.Name == stateName);
            return Switch(targetState);
        }
        /// <summary>
        /// �л�״̬
        /// </summary>
        /// <typeparam name="T">״̬����</typeparam>
        /// <returns>�л��ɷ���true ���򷵻�false</returns>
        public bool Switch<T>() where T : IState
        {
            return Switch(typeof(T).Name);
        }
        /// <summary>
        /// �л�����һ״̬
        /// </summary>
        public void Switch2Next()
        {
            if (states.Count != 0)
            {
                //�����ǰ״̬��Ϊ�� ����ݵ�ǰ״̬�ҵ���һ��״̬
                if (CurrentState != null)
                {
                    int index = states.IndexOf(CurrentState);
                    //��ǰ״̬������ֵ+1����С���б��е����� ����һ״̬������Ϊindex+1
                    //�����ʾ��ǰ״̬�Ѿ����б��е����һ�� ��һ״̬��ص��б��еĵ�һ��״̬ ����Ϊ0
                    index = index + 1 < states.Count ? index + 1 : 0;
                    IState targetState = states[index];
                    //����ִ�е�ǰ״̬���˳��¼� �ٸ��µ���һ״̬
                    CurrentState.OnExit();
                    CurrentState = targetState;
                }
                //��ǰ״̬Ϊ�� ��ֱ�ӽ����б��еĵ�һ��״̬
                else
                {
                    CurrentState = states[0];
                }
                //ִ��״̬�����¼�
                CurrentState.OnEnter();
            }
        }
        /// <summary>
        /// �л�����һ״̬
        /// </summary>
        public void Switch2Last()
        {
            if (states.Count != 0)
            {
                //�����ǰ״̬��Ϊ�� ����ݵ�ǰ״̬�ҵ���һ��״̬
                if (CurrentState != null)
                {
                    int index = states.IndexOf(CurrentState);
                    //��ǰ״̬������ֵ-1���������0 ����һ״̬������Ϊindex-1
                    //�����ʾ��ǰ״̬���б��еĵ�һ�� ��һ״̬��ص��б��е����һ��״̬
                    index = index - 1 >= 0 ? index - 1 : states.Count - 1;
                    IState targetState = states[index];
                    //����ִ�е�ǰ״̬���˳��¼� �ٸ��µ���һ״̬
                    CurrentState.OnExit();
                    CurrentState = targetState;
                }
                //��ǰ״̬Ϊ�� ��ֱ�ӽ����б��е����һ��״̬
                else
                {
                    CurrentState = states[states.Count - 1];
                }
                //ִ��״̬�����¼�
                CurrentState.OnEnter();
            }
        }
        /// <summary>
        /// �л�����״̬���˳���ǰ״̬��
        /// </summary>
        public void Switch2Null()
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit();
                CurrentState = null;
            }
        }
        /// <summary>
        /// ��ȡ״̬
        /// </summary>
        /// <typeparam name="T">״̬����</typeparam>
        /// <param name="stateName">״̬����</param>
        /// <returns>״̬</returns>
        public T GetState<T>(string stateName) where T : IState
        {
            return (T)states.Find(m => m.Name == stateName);
        }
        /// <summary>
        /// ��ȡ״̬
        /// </summary>
        /// <typeparam name="T">״̬����</typeparam>
        /// <returns>״̬</returns>
        public T GetState<T>() where T : IState
        {
            return (T)states.Find(m => m.Name == typeof(T).Name);
        }
        /// <summary>
        /// ״̬��ˢ���¼�
        /// </summary>
        public void OnUpdate()
        {
            //����ǰ״̬��Ϊ�� ִ��״̬ͣ���¼�
            CurrentState?.OnStay();
            //�������״̬�л�����
            for (int i = 0; i < conditions.Count; i++)
            {
                var condition = conditions[i];
                //��������
                if (condition.predicate.Invoke())
                {
                    //Դ״̬����Ϊ�� ��ʾ������״̬�л���Ŀ��״̬
                    if (string.IsNullOrEmpty(condition.sourceStateName))
                    {
                        Switch(condition.targetStateName);
                    }
                    //Դ״̬���Ʋ�Ϊ�� ��ʾ��ָ��״̬�л���Ŀ��״̬
                    else
                    {
                        //�����жϵ�ǰ��״̬�Ƿ�Ϊָ����״̬
                        if (CurrentState.Name == condition.sourceStateName)
                        {
                            Switch(condition.targetStateName);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// ״̬�������¼�
        /// </summary>
        public void OnDestroy()
        {
            //ִ��״̬��������״̬��״̬��ֹ�¼�
            for (int i = 0; i < states.Count; i++)
            {
                states[i].OnTermination();
            }
        }

        /// <summary>
        /// ����״̬�л�����
        /// </summary>
        /// <param name="predicate">�л�����</param>
        /// <param name="targetStateName">Ŀ��״̬����</param>
        /// <returns>״̬��</returns>
        public StateMachine SwitchWhen(Func<bool> predicate, string targetStateName)
        {
            conditions.Add(new StateSwitchCondition(predicate, null, targetStateName));
            return this;
        }
        /// <summary>
        /// ����״̬�л�����
        /// </summary>
        /// <param name="predicate">�л�����</param>
        /// <param name="sourceStateName">Դ״̬����</param>
        /// <param name="targetStateName">Ŀ��״̬����</param>
        /// <returns></returns>
        public StateMachine SwitchWhen(Func<bool> predicate, string sourceStateName, string targetStateName)
        {
            conditions.Add(new StateSwitchCondition(predicate, sourceStateName, targetStateName));
            return this;
        }

        /// <summary>
        /// ����״̬
        /// </summary>
        /// <typeparam name="T">״̬����</typeparam>
        /// <param name="stateName">״̬����</param>
        /// <returns>״̬������</returns>
        public StateBuilder<T> Build<T>(string stateName = null) where T : State, new()
        {
            Type type = typeof(T);
            T t = (T)Activator.CreateInstance(type);
            t.Name = string.IsNullOrEmpty(stateName) ? type.Name : stateName;
            if (states.Find(m => m.Name == t.Name) == null)
            {
                states.Add(t);
            }
            return new StateBuilder<T>(t, this);
        }

        /// <summary>
        /// ����״̬��
        /// </summary>
        /// <param name="stateMachineName">״̬������</param>
        /// <returns>״̬��</returns>
        public static StateMachine Create(string stateMachineName = null)
        {
            return StateManager.instance.Create<StateMachine>(stateMachineName);
        }
        /// <summary>
        /// ����״̬��
        /// </summary>
        /// <typeparam name="T">״̬������</typeparam>
        /// <param name="stateMachineName">״̬������</param>
        /// <returns>״̬��</returns>
        public static T Create<T>(string stateMachineName = null) where T : StateMachine, new()
        {
            return StateManager.instance.Create<T>(stateMachineName);
        }
        /// <summary>
        /// ����״̬��
        /// </summary>
        /// <param name="stateMachineName">״̬������</param>
        /// <returns>���ٳɹ�����true ���򷵻�false</returns>
        public static bool Destroy(string stateMachineName)
        {
            return StateManager.instance.Destroy(stateMachineName);
        }
        /// <summary>
        /// ����״̬��
        /// </summary>
        /// <typeparam name="T">״̬������</typeparam>
        /// <returns>���ٳɹ�����true ���򷵻�false</returns>
        public static bool Destroy<T>() where T : StateMachine
        {
            return StateManager.instance.Destroy(typeof(T).Name);
        }
        /// <summary>
        /// ��ȡ״̬��
        /// </summary>
        /// <param name="stateMachineName">״̬������</param>
        /// <returns>״̬��</returns>
        public StateMachine Get(string stateMachineName)
        {
            return StateManager.instance.GetMachine<StateMachine>(stateMachineName);
        }
        /// <summary>
        /// ��ȡ״̬��
        /// </summary>
        /// <typeparam name="T">״̬������</typeparam>
        /// <param name="stateMachineName">״̬������</param>
        /// <returns>״̬��</returns>
        public static T Get<T>(string stateMachineName) where T : StateMachine
        {
            return StateManager.instance.GetMachine<T>(stateMachineName);
        }
        /// <summary>
        /// ��ȡ״̬��
        /// </summary>
        /// <typeparam name="T">״̬������</typeparam>
        /// <returns>״̬��</returns>
        public static T Get<T>() where T : StateMachine
        {
            return StateManager.instance.GetMachine<T>(typeof(T).Name);
        }

}
}