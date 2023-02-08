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
        //单例类
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
            
            //事件
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneSwitching;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

        }


        #region 场景加载/卸载
        //todo：过渡场景？

        /// <summary>
        /// 预加载场景（LoadSceneMode.Additive）
        /// </summary>
        /// <param name="name"></param>
        public void PreLoadScene(string name)
        {

            pre = LoadSceneAsync(name);
            pre.allowSceneActivation = false;
            
        }

        /// <summary>
        /// 加载场景，并发送切换消息
        /// </summary>
        /// <param name="name">场景名称</param>
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
        /// 卸载场景
        /// </summary>
        /// <param name="name"></param>
        public void DestroyScene(string name)
        {
            var op=UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(name);
            op.allowSceneActivation = true;
        }
        #endregion

        //todo：资源加载（卸载/加载）？
        //todo：栈？
        //todo：场景切换效果？

        /// <summary>
        /// 获取当前活动场景
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
        /// 异步加载场景
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