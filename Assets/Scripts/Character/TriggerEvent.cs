using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    const string m_Collide = "Collide";
    const string m_PullCollide = "PullCollide";
    const string m_PushCollide = "PushCollide";

    public bool isColliding;
    public Collider2D collision;

    private void Awake()
    {
        MessageCenter.Instance.Register("M", OnMKeyDown);
        MessageCenter.Instance.Register("Space", OnSpaceKeyDown);
    }
    private void OnDestroy()
    {
        MessageCenter.Instance.Remove("M", OnMKeyDown);
        MessageCenter.Instance.Remove("Space", OnSpaceKeyDown);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        isColliding = true;
        this.collision= collision;
    }
    private void OnTriggerStay2D (Collider2D collision)
    {
        if (!isColliding)
        {
            isColliding = true;
            this.collision = collision;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
    }
    void OnMKeyDown(object obj)
    {
        if(isColliding)MessageCenter.Instance.Send(m_Collide, this.collision);
    }
    void OnSpaceKeyDown(object obj)
    {
        if (isColliding) MessageCenter.Instance.Send(m_PullCollide, this.collision.gameObject);
    }

}
