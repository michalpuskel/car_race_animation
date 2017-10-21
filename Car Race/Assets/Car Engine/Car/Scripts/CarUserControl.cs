using System;
using UnityEngine;

namespace CarEngine.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        private CarController m_Car; // the car controller we want to use


        private void Awake()
        {
            // get the car controller
            m_Car = GetComponent<CarController>();
        }


        private void FixedUpdate()
        {
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
