using System;
using UnityEngine;

using Scripts;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace CarEngine.Car
{
    [RequireComponent(typeof (CarController))]
    public class CarUserControl : MonoBehaviour
    {
        /**************************************************************/
        /**************************************************************/
        /**************************************************************/
        /**************************************************************/
        /**************************************************************/

        /// <summary>
        /// POZOR TODO DOLEZITE        
        /// </summary>
        /// 

        private const bool NAHRAVANIE_REDBULL = true;   // Tomas toto je 2. z 2 miest kde treba zmenit -> true / !true :D

        /// <summary>
        /// POZOR TODO DOLEZITE
        /// </summary>
        /// 

        /**************************************************************/
        /**************************************************************/
        /**************************************************************/
        /**************************************************************/
        /**************************************************************/
        /**************************************************************/


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

        private Rigidbody m_Rigidbody;
        private GameObject waterFX;

        private List<EllipsoidParticleEmitter> speedFXs;

        // /////////////////////////////

              
        private float timeFixed = 0.0f;
        private float time = 0.0f;

        public float interpolationPeriod = 0.1f;

        void Start()
        {
            speedFXs = new List<EllipsoidParticleEmitter>();

            m_Rigidbody = GetComponent<Rigidbody>();

            foreach (Transform child in transform) {
                if (child.CompareTag("WaterFX")) {
                    waterFX = child.gameObject;

                    foreach(Transform steamSpray in waterFX.transform)
                    {
                        foreach (Transform waterChild in steamSpray.transform)
                        {
                            speedFXs.Add(waterChild.GetComponent<EllipsoidParticleEmitter>());
                        }
                    }                    

                    break;
                }
            } 

            

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

            /*
            if (m_Rigidbody.velocity.magnitude * 2.23693629f > 5)
            {
                waterFX.SetActive(true);
            }
            else
            {
                waterFX.SetActive(false);
            }
            */
            
            
            foreach (EllipsoidParticleEmitter speedFX in speedFXs)
            {
                speedFX.maxEmission = speedFX.minEmission * ((m_Rigidbody.velocity.magnitude * 2.23693629f - 20) / 100);
            }
            


            if (m_InputController.InputIndex != CarType && !NAHRAVANIE_REDBULL)
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

                if (PlayAnimation)
                {
                    h = float.Parse(hh);
                    v = float.Parse(vv);

                    handbrake = float.Parse(bb);
                }
                else if (NAHRAVANIE_REDBULL)
                {
                    if (CarType == CustomInputController.Controllable.Ferrari)
                    {
                        h = 0;
                        v = 0;

                        handbrake = 0;
                    }
                    else
                    {
                        h = Input.GetAxis("Horizontal");
                        v = Input.GetAxis("Vertical");

                        handbrake = Input.GetAxis("Jump");
                    }
                }
                else
                {
                // not NAHRAVANIE_REDBULL
                    if (CarType == CustomInputController.Controllable.RedBull)
                    {
                        h = 0;
                        v = 0;

                        handbrake = 0;
                    }
                    else
                    {
                        h = Input.GetAxis("Horizontal");
                        v = Input.GetAxis("Vertical");

                        handbrake = Input.GetAxis("Jump");
                    }
                }                

                if (Recording && !NAHRAVANIE_REDBULL || Recording && NAHRAVANIE_REDBULL && CarType == CustomInputController.Controllable.RedBull)
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

                if (NAHRAVANIE_REDBULL)
                {
                    if (Recording && CarType ==  CustomInputController.Controllable.RedBull)
                    {
                        uiText.GetComponent<UnityEngine.UI.Text>().text = "Recording RB " + frameCounter;
                    }
                    else if (PlayAnimation && CarType == CustomInputController.Controllable.RedBull)
                    {
                        uiText.GetComponent<UnityEngine.UI.Text>().text = "PLAYing RB " + ll.Substring(10, ll.Length - 10);
                    }
                    else if (CarType == CustomInputController.Controllable.RedBull)
                    {
                        uiText.GetComponent<UnityEngine.UI.Text>().text = "RB " + frameCounter.ToString();
                    }
                }
                else
                {
                    if (Recording)
                    {
                        uiText.GetComponent<UnityEngine.UI.Text>().text = "Recording Fer " + frameCounter;
                    }
                    else if (PlayAnimation)
                    {
                        uiText.GetComponent<UnityEngine.UI.Text>().text = "PLAYing Fer " + ll.Substring(10, ll.Length - 10);
                    }
                    else
                    {
                        uiText.GetComponent<UnityEngine.UI.Text>().text = "Fer " + frameCounter.ToString();
                    }
                }

                

            
                

                /////////////////////        

                m_Car.Move(h, v, v, handbrake);

                //////                

            
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
