using System;
using UnityEngine;

using Scripts;
using System.IO;

namespace CarEngine.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        public CustomInputController.Controllable CarType;

        public Boolean Recording = false;
        public Boolean PlayAnimation;

        private CarController m_Car; // the car controller we want to use

        private CustomInputController m_InputController;

        private Camera m_Camera;
        private Vector3 m_OriginalCameraPosition;
        private Quaternion m_OriginalCameraRotation;

        private StreamWriter animation_log;        

        private GameObject uiText;
        private string filePath;
        private string filePathPlay;

        private StreamReader animation_play_log;

        void Start()
        {
            m_InputController = (CustomInputController)GameObject.FindGameObjectWithTag("InputController").GetComponent(typeof(CustomInputController));

            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_OriginalCameraRotation = m_Camera.transform.localRotation;

            filePath = "log/animation_log_" + CarType + ".txt";
            if (!File.Exists(filePath))
            {
                File.Create(filePath);                
            }            

            uiText = GameObject.FindGameObjectWithTag("recordingText");
            // animation_log = new StreamWriter(filePath, append: true);

            filePathPlay = "log/animation_log_" + CarType + "_PLAY.txt";

            if (File.Exists(filePathPlay))
            {
                File.Delete(filePathPlay);                
            }
            File.Copy(filePath, filePathPlay);

            animation_play_log = new StreamReader(filePathPlay);
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

            if (Input.GetKeyDown("p"))
            {
                Recording = !Recording;
            }
     

            if (Recording)
            {
                uiText.GetComponent<UnityEngine.UI.Text>().text = "Recording";
            }
            else
            {
                uiText.GetComponent<UnityEngine.UI.Text>().text = "";                
            }

            m_Camera.transform.localPosition = m_OriginalCameraPosition;
            m_Camera.transform.localRotation = m_OriginalCameraRotation;

            // pass the input to the car!

            string hh = null;
            string vv = null;
            string bb = null;
            string ll = null;

            if (PlayAnimation)
            {
                // animation_play_log = new StreamReader(filePathPlay);

                hh = animation_play_log.ReadLine();
                vv = animation_play_log.ReadLine();
                bb = animation_play_log.ReadLine();
                ll = animation_play_log.ReadLine();

                if (ll == null)
                {
                    PlayAnimation = false;
                    animation_play_log.Close();
                }                
            }            

            float h = (PlayAnimation && hh != null) ? float.Parse(hh) : Input.GetAxis("Horizontal");
            float v = (PlayAnimation && vv != null) ? float.Parse(vv) : Input.GetAxis("Vertical");

            float handbrake = (PlayAnimation && bb != null) ? float.Parse(bb) : Input.GetAxis("Jump");            

            if (Recording)
            {
                animation_log = new StreamWriter(filePath, append: true);

                animation_log.WriteLine(h);
                animation_log.WriteLine(v);
                animation_log.WriteLine(handbrake);
                animation_log.WriteLine();

                animation_log.Close();



                Debug.Log("Horizontal " + h);
                Debug.Log("Vertical " + v);
                Debug.Log("Handbrake " + handbrake);
            }            

            m_Car.Move(h, v, v, handbrake);
        }

        ~CarUserControl()
        {
            // animation_log.Close();
            // animation_play_log.Close();
        }
    }
}
