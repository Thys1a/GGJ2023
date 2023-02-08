using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{

    const string m_Collide = "Collide";
    private void OnTriggerEnter2D(Collider2D collision)
    {

        MessageCenter.Instance.Send(m_Collide, collision);
    }
}
