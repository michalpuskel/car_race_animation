using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWorksTriggerController : MonoBehaviour {

    private GameObject fireWorks;

	// Use this for initialization
	void Start () {
        fireWorks = GameObject.FindGameObjectWithTag("FireWorksMaster");
        fireWorks.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ferrari") || other.CompareTag("RedBull"))
        {
            fireWorks.SetActive(true);
        }        
    }
}
