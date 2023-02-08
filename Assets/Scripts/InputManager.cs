using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            MessageCenter.Instance.Send("E", null);
        }
    }
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MessageCenter.Instance.Send("Space", null);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            MessageCenter.Instance.Send("Q", null);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MessageCenter.Instance.Send("S", null);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            MessageCenter.Instance.Send("M", null);
        }
        
        if (Input.GetKeyDown(KeyCode.L))
        {
            MessageCenter.Instance.Send("L", null);
        }
    }
}
