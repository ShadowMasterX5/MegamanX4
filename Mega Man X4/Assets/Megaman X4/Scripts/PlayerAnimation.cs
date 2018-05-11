using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public PlayerStats playerStats;
    Animator animator;
    bool wasOnLadder;

    void Start()
    {
        animator = GetComponent<Animator>();
        wasOnLadder = playerStats.isOnLadder;
    }
    void LateUpdate()
    {
        CheckMovement();
        CheckJump();
        CheckGrounded();
        CheckLand();
        CheckShooting();
        //CheckLadder();
        wasOnLadder = playerStats.isOnLadder;
    }

    void CheckMovement()
    {
        animator.SetBool("isMoving", playerStats.isMoving);
    }

    void CheckJump()
    {
        if (playerStats.isJumping)
        {
            animator.SetTrigger("Jump");
            playerStats.isJumping = false;
        }
    }

    void CheckGrounded()
    {
        animator.SetBool("isGrounded", playerStats.isGrounded);

    }

    void CheckLand()
    {
        animator.SetBool("isToLand", playerStats.isToLand);
    }

    void CheckShooting()
    {
        if (playerStats.isShooting)
        {
            animator.SetBool("Shoot", playerStats.isShooting);
            playerStats.isShooting = false;
        }
    }


    //Animation Events

    void EnableControl()
    {
        playerStats.isControllable = true;
    }

    void DisableControl()
    {
        playerStats.isControllable = false;
    }
}