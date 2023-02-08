using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ½ÇÉ«¶¯»­¿ØÖÆÆ÷
/// </summary>
public class CharacterAnimationController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set(Animator animator)
    {
        this.animator = animator;
        
    }
    public void Walk()
    {
        animator.SetBool("isMoving", true);
    }
    public void StopWalking()
    {
        animator.SetBool("isMoving", false);
    }
    public void Jump()
    {
        animator.Play("Jump", 0, 0);//SetBool("isJumping", true);
    }
    public void StopJumping()
    {
        //animator.SetBool("isJumping", false);
    }
    public void Push()
    {
        animator.Play("Push", 0, 0);
    }
    public void StopPushing()
    {
        animator.SetBool("isPushing", false);
    }
    public void Pull()
    {
        animator.SetBool("isPulling", true);
    }
    public void StopPulling()
    {
        animator.SetBool("isPulling", false);
    }
    public void Succeed()
    {
        animator.Play("Succeed", 0, 0);
        StopPulling();
    }
    public void Fail()
    {
        animator.Play("Fail", 0, 0);
        StopPulling();
    }
    public void ReturnToIdle()
    {
        animator.SetBool("isSucceeding", false);
        animator.SetBool("isFailing", false);
        animator.SetBool("isPulling", false);
    }
    public void GetNote()
    {
        animator.SetBool("isGettingNote", true);
    }
    public void StopGettingNote()
    {
        animator.SetBool("isGettingNote", false);

    }
    public void LaunchNote()
    {
        animator.Play("LaunchNote", 0, 0);
    }
}
