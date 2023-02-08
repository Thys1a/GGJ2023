using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UFramework
{
    public class SceneManager
    {
        //������
        private static SceneManager instance;
        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SceneManager();
                }
                return instance;
            }
        }
        private AsyncOperation pre;
        private Stack<string > sceneStack;

        private SceneManager()
        {
            
            //�¼�
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneSwitching;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        }


        #region ��������/ж��
        //todo�����ɳ�����

        /// <summary>
        /// Ԥ���س�����LoadSceneMode.Additive��
        /// </summary>
        /// <param name="name"></param>
        public void PreLoadScene(string name)
        {

            pre = LoadSceneAsync(name);
            pre.allowSceneActivation = false;
            
        }

        /// <summary>
        /// ���س������������л���Ϣ
        /// </summary>
        /// <param name="name">��������</param>
        public void LoadScene(string name = null)
        {
            if (name == null)
            {
                if (pre!=null)
                {
                    
                    pre.allowSceneActivation = true;
                }
                else
                {
                    Debug.Log("scene name was not provided");
                    return;
                }
            }
            else
            {
                if (pre != null)
                {

                    pre.allowSceneActivation = true;
                    Debug.Log("preloaded");
                }
                else
                {
                    AsyncOperation op = LoadSceneAsync(name);
                    
                }
                
                //op.allowSceneActivation=true;
            }
        }

        public void LoadDeeperScene(string name = null)
        {
            sceneStack.Push(GetScene().name);
            if (name == null)
            {
                if (pre != null)
                {

                    pre.allowSceneActivation = true;
                }
                else
                {
                    Debug.LogError("scene name was not provided");
                    return;
                }
            }
            else
            {
                AsyncOperation op = LoadSceneAsync(name);
                
            }
        }
        public void ReturnToLastScene()
        {
            if (sceneStack.Count > 0) LoadSceneAsync(sceneStack.Pop());
        }

        

        /// <summary>
        /// ж�س���
        /// </summary>
        /// <param name="name"></param>
        public void DestroyScene(string name)
        {
            var op=UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(name);
            op.allowSceneActivation = true;
        }
        #endregion

        //todo����Դ���أ�ж��/���أ���
        //todo��ջ��
        //todo�������л�Ч����

        /// <summary>
        /// ��ȡ��ǰ�����
        /// </summary>
        /// <returns></returns>
        public Scene GetScene()
        {

            return UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        }

        public int LoadedSceneCount()
        {
            return UnityEngine.SceneManagement.SceneManager.sceneCount;
        }
        public void ClearPreLoadingScene()
        {
            pre = null;
        }

        /// <summary>
        /// �첽���س���
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private AsyncOperation LoadSceneAsync(string name)
        {
            AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
            return op;
        }

        #region Message
        private void OnSceneSwitching(Scene current, Scene next)
        {
            Debug.Log(current + " to " + next);
            //MessageCenter.Instance.Send(MessageCenter.MessageType.SceneSwitching, (object)next);
        }
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log("OnSceneLoaded: " + scene.name + ";mode: " + mode);
        }
        private void OnSceneUnloaded(Scene current)
        {
            Debug.Log("OnSceneUnloaded: " + current);
        }
        #endregion
    }
}