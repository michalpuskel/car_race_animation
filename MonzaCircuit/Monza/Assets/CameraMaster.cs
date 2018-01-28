using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMaster : MonoBehaviour {
    private Dictionary<string, GameObject> cameras;
    private RainScript rain;

	// Use this for initialization
	void Start () {
        rain = GameObject.FindGameObjectWithTag("superRain").GetComponent<RainScript>();

        cameras = new Dictionary<string, GameObject>();
        for (byte i = 0; i < 10; i++)
        {
            cameras["autokam_" + i] = GameObject.FindGameObjectWithTag("autokam_" + i);
        }

        for (byte i = 0; i < 10; i++)
        {
            if (i != 1)            
            {
                cameras["autokam_" + i].SetActive(false);
            }                       
        }

        rain.Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        for (byte i = 0; i < 10; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                for (byte ii = 0; ii < 10; ii++)
                {
                    if (i == ii)
                    {
                        cameras["autokam_" + ii].SetActive(true);                        
                    }
                    else {
                        cameras["autokam_" + ii].SetActive(false);
                    }
                }
            }
        }

        rain.Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
}
