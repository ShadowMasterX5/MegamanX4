using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerStats playerStats;
    public float speed = 3.2f;
    Rigidbody2D playerRigidbody;
    Vector2 speedVector;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStats.facingRight = true;
        speedVector = Vector2.zero;
    }

    void FixedUpdate()
    {
        if (playerStats.isControllable && !playerStats.isOnLadder)
        {
            Movement();
        }
    }

    void Movement()
    {
        CheckFlip();
        speedVector.y = playerRigidbody.velocity.y;
        if (Input.GetButton("Horizontal"))
        {
            PlayerMoves();
        }
        else
        {
            PlayerStops();
        }
    }

    void CheckFlip()
    {
        if ((playerStats.facingRight && (Input.GetAxis("Horizontal") < 0f)) ||
            (!playerStats.facingRight && (Input.GetAxis("Horizontal") > 0f)))
        {

            Flip();
        }
    }

    void PlayerMoves()
    {
        speedVector.x = speed * ((Input.GetAxis("Horizontal") > 0f) ? 1f : -1f);
        playerRigidbody.velocity = speedVector;
        playerStats.isMoving = true;
    }

    void PlayerStops()
    {
        speedVector.x = 0f;
        playerRigidbody.velocity = speedVector;
        playerStats.isMoving = false;
    }

    void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
        playerStats.facingRight = !playerStats.facingRight;
    }

}