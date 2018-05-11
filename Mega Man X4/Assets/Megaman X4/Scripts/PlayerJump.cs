using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    #region  Variables
    [SerializeField] PlayerStats playerStats;
    [SerializeField] LayerMask levelLayer;
    [SerializeField] float jumpForce = 10000f;
    [SerializeField] float minJumpSpeed = 3f;
    [SerializeField] float fallGravity = 3f;
    Rigidbody2D playerRigidbody;
    const float landCheckOffset = 1.5f;
    const float groundCheckOffset = 0.55f;
    const float footOffset = 0.23f;
    #endregion

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckLandingAnimStart();
        CheckGrounded();
        if (playerStats.isControllable)
        {
            Jump();
        }
    }

    void CheckLandingAnimStart()
    {
        if ((CheckFoot(landCheckOffset)) &&
            (playerRigidbody.velocity.y <= 0f))
        {

            playerStats.isToLand = true;
        }
        else
        {
            playerStats.isToLand = false;
        }
    }

    bool CheckFoot(float groundOffset)
    {
        Vector2 leftFootPosition = transform.position - new Vector3(footOffset, 0f, 0f);
        Vector2 rightFootPosition = transform.position + new Vector3(footOffset, 0f, 0f);
        return (Physics2D.Raycast(leftFootPosition, Vector2.down, groundOffset, levelLayer)
        || Physics2D.Raycast(rightFootPosition, Vector2.down, groundOffset, levelLayer));
    }

    void CheckGrounded()
    {
        if (CheckFoot(groundCheckOffset))
        {
            playerStats.isGrounded = true;
        }
        else
        {
            playerStats.isGrounded = false;
        }
    }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump")) && playerStats.isGrounded &&
            !playerStats.isOnLadder)
        {

            Vector2 jumpVector = new Vector2(0f, jumpForce);
            playerRigidbody.AddForce(jumpVector);
            playerStats.isJumping = true;
        }
        FallController();
    }

    void FallController()
    {
        if (!Input.GetButton("Jump") && playerRigidbody.velocity.y > minJumpSpeed)
        {
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, minJumpSpeed);
        }
        else if (!playerStats.isOnLadder)
        {
            RegulateFallGravity();
        }
    }

    void RegulateFallGravity()
    {
        if (playerRigidbody.velocity.y < 0f)
        {
            playerRigidbody.gravityScale = fallGravity;
        }
        else
        {
            playerRigidbody.gravityScale = 3f;
        }
    }
}
