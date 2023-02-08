using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UFramework
{
    /// <summary>
    /// ��ɫ����ģ�黯���
    /// </summary>
    public class CharacterManager
    {
        private Character character;
        private GameObject gameObject;
        private List<Character> characters=new List<Character>();
        private static CharacterManager instance;

        //ȫ�ֹ㲥�¼�
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
        /// ������ɫ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="character">��ɫobject���ϵ�character�ű�</param>
        /// <returns></returns>
        public CharacterManager CreateCharacter<T>(Character character) 
        {
            this.character = character;
            this.gameObject = character.gameObject;
            characters.Add(character);
            return this;
        }
        /// <summary>
        /// ������
        /// </summary>
        /// <typeparam name="T">���</typeparam>
        /// <returns></returns>
        public CharacterManager Add<T>() where T:Component
        {
            gameObject.AddComponent<T>();
            return this;
        }
        /// <summary>
        /// �������
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