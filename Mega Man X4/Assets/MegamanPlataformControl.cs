using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(MegamanPlataform))]
    public class MegamanPlataformControl : MonoBehaviour
    {
        private MegamanPlataform m_Character;
        private bool m_Jump;


        private void Awake()
        {
            m_Character = GetComponent<MegamanPlataform>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            float h = CrossPlatformInputManager.GetAxisRaw("Horizontal");

            // Pass all parameters to the character control script.
          
            //m_Jump = false;
        }
    }
}
