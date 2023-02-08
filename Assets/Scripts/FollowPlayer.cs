using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private float left,right,up,down;
    private GameObject player;
    private Vector3 pos;
    public float moving_speed;

    // Start is called before the first frame update
    void Start()
    {
        GameObject background = GameObject.FindGameObjectWithTag("Background");
        if (background != null)
        {
            left = background.GetComponent<SpriteRenderer>().bounds.min.x;
            down = background.GetComponent<SpriteRenderer>().bounds.min.y;
            right = background.GetComponent<SpriteRenderer>().bounds.max.x;
            up = background.GetComponent<SpriteRenderer>().bounds.max.y;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        pos = this.transform.position;
        var delx = player.transform.position.x - pos.x;
        var dely = player.transform.position.y - pos.y;
        pos.x += delx * moving_speed * Time.fixedDeltaTime;
        //if (pos.x > right) pos.x = right;
        //if (pos.x < left) pos.x = left;
        pos.x = Mathf.Clamp(pos.x,left+9.8f, right-9.8f);
        pos.y += dely * moving_speed * Time.fixedDeltaTime;
        //if (pos.y > up) pos.y = up;
        //if (pos.y < down) pos.y = down;
        pos.y = Mathf.Clamp(pos.y, down+5.4f, up-5.4f);
        this.transform.position = Vector3.MoveTowards(transform.position, pos, Time.fixedDeltaTime);
    }
}
