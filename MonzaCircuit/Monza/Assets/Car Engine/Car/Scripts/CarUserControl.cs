using System;
using UnityEngine;

using Scripts;
using System.IO;
using System.Collections;

namespace CarEngine.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        public CustomInputController.Controllable CarType;

        public Boolean Recording = false;
        public Boolean PlayAnimation = false;

        private CarController m_Car; // the car controller we want to use

        private CustomInputController m_InputController;

        private Camera m_Camera;
        private Vector3 m_OriginalCameraPosition;
        private Quaternion m_OriginalCameraRotation;

        public StreamWriter animation_log;        

        private GameObject uiText;
        public string filePath;
        public string filePathPlay;

        public StreamReader animation_play_log;

        private UInt64 frameCounter = 0;

        private float h;
        private float v;
        private float handbrake;

        // /////////////////////////////

              
        private float timeFixed = 0.0f;
        private float time = 0.0f;

        public float interpolationPeriod = 0.1f;

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

            // animation_play_log = new StreamReader(filePathPlay);

            // StartCoroutine("DoCheck");            
        }

        private void Awake()
        {
            // get the car controller

            m_Car = GetComponent<CarController>();            
        }          

        private void Update()
        {


            /*
            time += Time.deltaTime;

            if (time >= interpolationPeriod)
            {
                time = time - interpolationPeriod;
                */

                // execute block of code here
                

                if (m_InputController.InputIndex != CarType)
                {
                    return;
                }

                if (Input.GetKeyDown("p"))
                {
                    Recording = !Recording;

                    if (!Recording)
                    {
                        animation_log.Close();
                    }
                    else
                    {
                        animation_log = new StreamWriter(filePath, append: true);
                    }
                }

                if (Input.GetKeyDown("b"))
                {
                    PlayAnimation = true;
                    animation_play_log = new StreamReader(filePathPlay);
                }

                m_Camera.transform.localPosition = m_OriginalCameraPosition;
                m_Camera.transform.localRotation = m_OriginalCameraRotation;

                // pass the input to the car!

                ///////////////////

                

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
                else
                {
                    //Recording = true;
                    //animation_log = new StreamWriter(filePath, append: true);
                }

                h = (PlayAnimation && hh != null) ? float.Parse(hh) : Input.GetAxis("Horizontal");
                v = (PlayAnimation && vv != null) ? float.Parse(vv) : Input.GetAxis("Vertical");

                handbrake = (PlayAnimation && bb != null) ? float.Parse(bb) : Input.GetAxis("Jump");

                if (Recording)
                {
                    // animation_log = new StreamWriter(filePath, append: true);

                    animation_log.WriteLine(h);
                    animation_log.WriteLine(v);
                    animation_log.WriteLine(handbrake);
                    animation_log.WriteLine("          " + frameCounter.ToString());

                    frameCounter++;

                    // animation_log.Close();        


                    Debug.Log("Horizontal " + h);
                    Debug.Log("Vertical " + v);
                    Debug.Log("Handbrake " + handbrake);
                }

                if (Recording)
                {
                    uiText.GetComponent<UnityEngine.UI.Text>().text = "Recording " + frameCounter;
                }
                else if (PlayAnimation)
                {
                    uiText.GetComponent<UnityEngine.UI.Text>().text = "PLAYing " + ll.Substring(10, ll.Length - 10);
                }
                else
                {
                    uiText.GetComponent<UnityEngine.UI.Text>().text = frameCounter.ToString();
                }

            
                

                /////////////////////

                m_Car.Move(h, v, v, handbrake);

            
          //  } // time if

           
        }

        /*
        private void FixedUpdate()
        {
            timeFixed += Time.deltaTime;

            if (timeFixed >= interpolationPeriod)
            {
                timeFixed = timeFixed - interpolationPeriod;

                // execute block of code here

                if (m_InputController.InputIndex != CarType)
                {
                    return;
                }

                m_Car.Move(h, v, v, handbrake);
            }            
        }
        */

        
            /*
        private IEnumerator DoCheck()
        {
            for (; ; )
            {
                // execute block of code here

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

                h = (PlayAnimation && hh != null) ? float.Parse(hh) : Input.GetAxis("Horizontal");
                v = (PlayAnimation && vv != null) ? float.Parse(vv) : Input.GetAxis("Vertical");

                handbrake = (PlayAnimation && bb != null) ? float.Parse(bb) : Input.GetAxis("Jump");

                if (Recording)
                {
                    // animation_log = new StreamWriter(filePath, append: true);

                    animation_log.WriteLine(h);
                    animation_log.WriteLine(v);
                    animation_log.WriteLine(handbrake);
                    animation_log.WriteLine();

                    frameCounter++;

                    // animation_log.Close();        


                    Debug.Log("Horizontal " + h);
                    Debug.Log("Vertical " + v);
                    Debug.Log("Handbrake " + handbrake);
                }

                if (Recording)
                {
                    uiText.GetComponent<UnityEngine.UI.Text>().text = "Recording " + frameCounter;
                }
                else if (PlayAnimation)
                {
                    uiText.GetComponent<UnityEngine.UI.Text>().text = "PLAYing";
                }
                else
                {
                    uiText.GetComponent<UnityEngine.UI.Text>().text = frameCounter.ToString();
                }

                //m_Car.Move(h, v, v, handbrake);

                /////////////////////////////////////////////////////////////////////////////////////

                yield return new WaitForSeconds(interpolationPeriod);
            }
        }*/
        
    

        ~CarUserControl()
        {
            // animation_log.Close();
            // animation_play_log.Close();
        }
    }
}
