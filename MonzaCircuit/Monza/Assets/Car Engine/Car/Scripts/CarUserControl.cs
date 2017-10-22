using System;
using UnityEngine;

using Scripts;

namespace CarEngine.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        public CustomInputController.Controllable CarType;

        private CarController m_Car; // the car controller we want to use

        private CustomInputController m_InputController;

        private Camera m_Camera;
        private Vector3 m_OriginalCameraPosition;
        private Quaternion m_OriginalCameraRotation;

        void Start()
        {
            m_InputController = (CustomInputController)GameObject.FindGameObjectWithTag("InputController").GetComponent(typeof(CustomInputController));

            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_OriginalCameraRotation = m_Camera.transform.localRotation;
        }

        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
            if (m_InputController.InputIndex != CarType) {
                return;
            }

            m_Camera.transform.localPosition = m_OriginalCameraPosition;
            m_Camera.transform.localRotation = m_OriginalCameraRotation;

            // pass the input to the car!
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            float handbrake = Input.GetAxis("Jump");

            Debug.Log("Horizontal " + h);
            Debug.Log("Vertical " + v);
            Debug.Log("Handbrake " + handbrake);

            m_Car.Move(h, v, v, handbrake);
        }
    }
}
