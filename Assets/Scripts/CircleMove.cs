using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    private float left, right, up, down;
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
        //mask.
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            var playerPos = player.transform.position;
            var circleBounds = this.GetComponent<SpriteRenderer>().bounds;
            if (playerPos.x > circleBounds.max.x || playerPos.y > circleBounds.max.y || playerPos.x < circleBounds.min.x || playerPos.y < circleBounds.min.y) return;
            print("push");
            player.GetComponent<MovingController>().animationController.Push();
            if (Input.GetKey(KeyCode.W))
            {
                Move(0);
            }
            else if(Input.GetKey(KeyCode.S))
            {
                Move(1);
            }
            else if(Input.GetKey(KeyCode.A))
            {
                Move(2);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                Move(3);
            }
            else
            {
                if(player.transform.localScale.x>0) Move(3);
                else if (player.transform.localScale.x < 0) Move(2);
            }
        }
    }
    private void Move(int dir) {
        
        pos = this.transform.position;

        Vector2 del;
        switch (dir)
        {
            case 0:del = Vector2.up;break;
            case 1: del = Vector2.down; break;
            case 2: del = Vector2.left; break;
            case 3: del = Vector2.right; break;
            default:del = Vector2.zero;break;
        }
        pos.x += del.x * moving_speed;
        pos.x = Mathf.Clamp(pos.x, left, right);
        
        pos.y += del.y * moving_speed;
        pos.y = Mathf.Clamp(pos.y, down, up);
        Tween twe = this.transform.DOMove( pos, 1.5f);
        //twe.OnComplete(Stop);
        
    }

    private void Stop()
    {
        player.GetComponent<MovingController>().animationController.StopPushing();
    }
}
