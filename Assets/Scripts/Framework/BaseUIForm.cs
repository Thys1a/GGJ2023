using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UFramework
{
    public class BaseUIForm : MonoBehaviour
    {

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        //页面显示
        public virtual void Display()
        {
            UIManager.Instance.SetUIMask(this.gameObject);
            this.gameObject.SetActive(true);
        }

        //页面隐藏(不在“栈”集合中)
        public virtual void Hiding()
        {
            UIManager.Instance.CancelUIMask(this.gameObject);
            this.gameObject.SetActive(false);
        }
        //页面重新显示
        public virtual void Redisplay()
        {
            UIManager.Instance.SetUIMask(this.gameObject);
            this.gameObject.SetActive(true);
        }
        //页面冻结(还在“栈”集合中)
        public virtual void Freeze()
        {
            this.gameObject.SetActive(true);
        }
    }
}
