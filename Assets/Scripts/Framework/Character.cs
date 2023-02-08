using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFramework
{
    /// <summary>
    /// �����ɫ��
    /// </summary>
    public abstract class Character : MonoBehaviour
    {
        protected string characterName;
        protected LocalConfig config;

        public virtual void SetConfig(LocalConfig config)
        {
            this.config = config;
        }
    }
}