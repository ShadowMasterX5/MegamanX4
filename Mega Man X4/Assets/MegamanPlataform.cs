using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class MegamanPlataform : MonoBehaviour
    {
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%  
        [SerializeField] PlayerStats playerStats;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
        [SerializeField] float jumpForce = 13500f;
        [SerializeField] float minJumpSpeed = 3f;
        [SerializeField] float fallGravity = 3f;
        [SerializeField] float speed = 6f;
        [SerializeField] GameObject shotPrefab;
        [SerializeField] Transform jumpShotStartPoint;
        [SerializeField] Transform runningShotStartPoint;
        [SerializeField] Transform standShotStartPoint;
        [SerializeField] int shotAmount = 3;
        [SerializeField] float shootSpeed = 1f;
        [SerializeField] float damage = 1f;
        [SerializeField] float lifetime = 1f;
        


        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private bool m_isFiring;
        private bool m_isToLand;
        private bool m_isJumping;
        private bool m_isMoving;
        private bool m_FacingRight = true;
        private bool m_isControllable = true;
        private bool m_isShooting;

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private Vector2 speedVector;
        private GameObject[] shotPool;

        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        const float landCheckOffset = 0.5f;
        const float groundCheckOffset = 0.55f;
        const float footOffset = 0.23f;


        private void Start()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            speedVector = Vector2.zero;
            playerStats.facingRight = true;
        }

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            shotPool = new GameObject[shotAmount];

            for (int i = 0; i < shotPool.Length; i++)
            {
                shotPrefab.transform.localScale.Set(5.0f,5.0f,1.0f);
                shotPool[i] = Instantiate(shotPrefab);
                shotPool[i].SetActive(false);
            }
        }

        private void Update()
        {
            CheckGrounded();
            CheckLandingAnimStart();
            if (m_isControllable)
            {
                Jump();
                Movement();
                ShootController();
            }
        }

        void LateUpdate()
        {
            CheckMovement();
            CheckJump();
            CheckisGrounded();
            CheckLand();
            CheckShooting();
        }

        private void CheckMovement()
        {
            m_Anim.SetBool("isMoving", m_isMoving);
        }

        private void CheckJump()
        {
            if (m_isJumping)
            {
                m_Anim.SetTrigger("Jump");
                m_isJumping = false;
            }
        }

        private void CheckisGrounded()
        {
            m_Anim.SetBool("isGrounded", m_Grounded);
        }
        
        private void CheckLand()
        {
            m_Anim.SetBool("isToLand", m_isToLand);
        }

        private void CheckShooting()
        {
            if (m_isShooting)
            {
                m_Anim.SetBool("Shoot", m_isShooting);
                m_isShooting = false;
            }
        }

        void EnableControl()
        {
            m_isControllable = true;
        }

        void DisableControl()
        {
            m_isControllable = false;
        }


        private void FixedUpdate()
        {

        }

        void Movement()
        {
            CheckFlip();
            speedVector.y = m_Rigidbody2D.velocity.y;
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
            m_Rigidbody2D.velocity = speedVector;
            m_isMoving = true;
        }

        void PlayerStops()
        {
            speedVector.x = 0f;
            m_Rigidbody2D.velocity = speedVector;
            m_isMoving = false;
        }



        private void Flip()
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
            playerStats.facingRight = !playerStats.facingRight;
        }


        void CheckLandingAnimStart()
        {
            if ((CheckFoot(landCheckOffset)) && (m_Rigidbody2D.velocity.y <= 0f))
            {
                m_isToLand = true;
            }
            else
            {
                m_isToLand = false;
            }

        }

        void CheckGrounded()
        {
            if (CheckFoot(groundCheckOffset))
            {
                m_Grounded = true;
            }
            else
            {
                m_Grounded = false;
            }
            
        }

        bool CheckFoot(float groundOffset)
        {
            Vector2 leftFootPosition = transform.position - new Vector3(footOffset, 0f, 0f);
            Vector2 rightFootPosition = transform.position + new Vector3(footOffset, 0f, 0f);
            
            return (Physics2D.Raycast(leftFootPosition, Vector2.down, groundOffset, m_WhatIsGround)
                    || Physics2D.Raycast(rightFootPosition, Vector2.down, groundOffset, m_WhatIsGround));
        }


        void Jump()
        {
            if ((Input.GetButtonDown("Jump")) && m_Grounded )    
            {
                Vector2 jumpVector = new Vector2(0f, jumpForce);
                m_Rigidbody2D.AddForce(jumpVector);
                m_isJumping = true;
            }
            FallController();
        }

        void FallController()
        {
            if (!Input.GetButton("Jump") && m_Rigidbody2D.velocity.y > minJumpSpeed)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, minJumpSpeed);
            }
            else
            {
                RegulateFallGravity();
            }
    }

        void RegulateFallGravity()
        {
            if (m_Rigidbody2D.velocity.y < 0f)
            {
                m_Rigidbody2D.gravityScale = fallGravity;
            }
            else
            {
                m_Rigidbody2D.gravityScale = 3f;
            }
        }

        void ShootController()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShotPooling();
                Debug.Log("Atirei");
            }
        }

        void ShotPooling()
        {
            int shotIndex = 0;
            while (CurrentShotIsBusy(shotIndex))
            {
                shotIndex++;
                if (shotIndex >= shotAmount)
                {
                    print("Sin Balas!");
                    break;
                }
            }
            if (shotIndex < shotAmount)
            {
                m_isShooting = true;
                SetupShot(shotPool[shotIndex]);
            }
        }

        bool CurrentShotIsBusy(int index)
        {
            return shotPool[index].activeInHierarchy;
        }

        void SetupShot(GameObject shot)
        {
            Transform shotStartPoint;
            shotStartPoint = GetShotStartPoint();
            shot.SetActive(true);
            shot.transform.position = shotStartPoint.position;
            shot.transform.rotation = shotStartPoint.rotation;
            shot.transform.localScale = shotStartPoint.parent.localScale; //parent porque megaman(padre del componente)
            //usa x-scale -1 para girar
        }

        Transform GetShotStartPoint()
        {
            if (!m_Grounded)
            {
                return jumpShotStartPoint;
            }
            else if (m_isMoving)
            {
                return runningShotStartPoint;
            }
            else
            {
                return standShotStartPoint;
            }
        }

        
    }
}
