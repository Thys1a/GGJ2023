using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UFramework
{
    /// <summary>
    /// 角色管理：模块化组成
    /// </summary>
    public class CharacterManager
    {
        private Character character;
        private GameObject gameObject;
        private List<Character> characters=new List<Character>();
        private static CharacterManager instance;

        //全局广播事件
        const string m_characterCompleted = "CharacterCompleted";

        public static CharacterManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CharacterManager();
                }
                return instance;
            }
        }
        #region Builder
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="character">角色object身上的character脚本</param>
        /// <returns></returns>
        public CharacterManager CreateCharacter<T>(Character character) 
        {
            this.character = character;
            this.gameObject = character.gameObject;
            characters.Add(character);
            return this;
        }
        /// <summary>
        /// 添加组件
        /// </summary>
        /// <typeparam name="T">组件</typeparam>
        /// <returns></returns>
        public CharacterManager Add<T>() where T:Component
        {
            gameObject.AddComponent<T>();
            return this;
        }
        /// <summary>
        /// 添加设置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public CharacterManager Add(LocalConfig config) 
        {
            character.SetConfig(config);
            return this;
        }
        public Character GetCharacter()
        {
            MessageCenter.Instance.Send(m_characterCompleted, null);
            return character;
        }
        public GameObject GetGameObject()
        {
            return gameObject;
        }
        #endregion
    }
}