﻿using CarEngine.Car;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityStandardAssets.Characters.FirstPerson;

namespace Scripts
{
    public class CustomInputController : MonoBehaviour
    {
        public enum Controllable { FPS, RedBull, Ferrari };
        public Controllable InputIndex;
        private GameObject FPS;
        private GameObject FerrariCam;
        private GameObject RedBullCam;

        // Use this for initialization
        void Start()
        {
            InputIndex = Controllable.FPS;
            FPS = GameObject.FindGameObjectWithTag("FPS");
            FerrariCam = GameObject.FindGameObjectWithTag("FerrariCam");
            RedBullCam = GameObject.FindGameObjectWithTag("RedBullCam");

            StartCoroutine(SwitchCar());
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetAxis("Input_0") > 0)
            {
                InputIndex = Controllable.FPS;                
                Debug.Log("FPS");

                FPS.SetActive(true);
                FerrariCam.SetActive(false);
                RedBullCam.SetActive(false);                
            }
            else if (Input.GetAxis("Input_1") > 0)
            {
                InputIndex = Controllable.RedBull;
                Debug.Log("RedBull");

                FPS.SetActive(false);
                FerrariCam.SetActive(false);
                RedBullCam.SetActive(true);                
            }
            else if (Input.GetAxis("Input_2") > 0)
            {
                InputIndex = Controllable.Ferrari;
                Debug.Log("Ferrari");

                FPS.SetActive(false);
                FerrariCam.SetActive(true);
                RedBullCam.SetActive(false);                
            }

            /*
            FirstPersonController fps = GameObject.FindGameObjectWithTag("FPS").GetComponent<FirstPersonController>();
            fps.m_Camera = Camera.main;
            fps.m_OriginalCameraPosition = fps.m_Camera.transform.localPosition;
            */
        }

        IEnumerator SwitchCar()
        {            
            yield return new WaitForSeconds(2);

            InputIndex = Controllable.Ferrari;
            Debug.Log("Ferrari");

            FPS.SetActive(false);
            FerrariCam.SetActive(true);
            RedBullCam.SetActive(false);

            StartCoroutine(StartRecord());
        }

        IEnumerator StartRecord()
        {
            yield return new WaitForSeconds(1);

            var auto = GameObject.FindGameObjectWithTag("Ferrari").GetComponent<CarUserControl>();


            var xor = !true;

            ///////////////////

            if (xor)
            {           
                auto.Recording = true;        
                auto.animation_log = new StreamWriter(auto.filePath, append: true);
            }           
            else
            {
                auto.PlayAnimation = true;
                auto.animation_play_log = new StreamReader(auto.filePathPlay);
            }
            
        }
    }

}

