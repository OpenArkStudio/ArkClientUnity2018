using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public class MoverCtrl : MonoBehaviour
    {
        DataNodeComponent m_dataNodeComponent;
        private Mover m_mover; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        float m_h = 0f;
        float m_v = 0f;
        int tempCount;
        // Use this for initialization
        void Start()
        {
            m_Cam = ARKGameEntry.Scene.MainCamera.transform;
            m_mover = GetComponent<Mover>();
            if (m_mover == null)
            {
                Log.Error("(m_mover == null)!");
            }
            m_dataNodeComponent = ARKGameEntry.DataNode;
            //0.2fs request move once;
            InvokeRepeating("RequestMove", 0f, 0.2f);
        }

        private void RequestMove()
        {
            // read inputs
            float h=0f, v=0f;
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");
            }
            else
            {
                h = m_dataNodeComponent.GetData<VarFloat>(Constant.DataNodeData.ScreenDirectionX);
                v = m_dataNodeComponent.GetData<VarFloat>(Constant.DataNodeData.ScreenDirectionY);
            }
            
            
            SetPosition(transform.position);
            if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
            {
                tempCount = 0;
                Log.Info("Move Ctrl h=" + h + ", v=" + v);
                //ARKGameEntry.AFNet.RequireMove(ARKGameEntry.AFData.m_selfRoleID, h, v, transform.position);
                Set(h, v);
            }
            else
            {
                if (tempCount == 0)
                {
                    //ARKGameEntry.AFNet.RequireMove(ARKGameEntry.AFData.m_selfRoleID, 0f, 0f, transform.position);
                    Set(0f, 0f);
                }
                tempCount++;
            }

        }
        public void Set(float h, float v)
        {
            m_h = h;
            m_v = v;
        }
        private void Update()
        {
            if (!m_Jump)
            {
                m_Jump = Input.GetButtonDown("Jump");
            }
            MoveUpdate(m_h, m_v);
        }
        public void SetPosition(Vector3 pos)
        {
            m_mover.SetPosition(pos);
        }
        // Fixed update is called in sync with physics
        private void MoveUpdate(float h, float v)
        {

            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
			// walk speed multiplier
	        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_mover.Move(m_Move, crouch, m_Jump);
            m_Jump = false;
        }
    }
}