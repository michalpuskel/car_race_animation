using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovieCamFPSController : MonoBehaviour {

    public float MoveSpeed;
    public float LookSpeed;    

    private Rigidbody rb;
    private Quaternion lookCamera;    

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        lookCamera = Quaternion.identity;

        //Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = lookCamera * new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement.Set(
            movement.x,
            0.0f,
            movement.z
        );
       
        movement = Camera.main.transform.TransformDirection(movement);
        //movement.y = 0.0f;

        rb.velocity = movement * MoveSpeed;
        rb.rotation = Quaternion.identity;        
    }

    void LateUpdate()
    {
        float lookHorizontal = Input.GetAxis("Mouse X");
        float lookVertical = Input.GetAxis("Mouse Y");

        Vector3 lookCam = new Vector3(-lookVertical, lookHorizontal, 0.0f) * LookSpeed;
        lookCamera *= Quaternion.Euler(lookCam);
        lookCamera = Quaternion.Euler(new Vector3
        (
            ClampAngle(lookCamera.eulerAngles.x, 60.0f),
            lookCamera.eulerAngles.y,
            0.0f
        ));

        transform.rotation = lookCamera;
    }

    float ClampAngle(float angle, float deltaMax)
    {
        if (angle < 180.0f)
        {
            return Mathf.Clamp(angle, 0, deltaMax);
        }
        return Mathf.Clamp(angle, 360.0f - deltaMax, 360.0f);
    }
}
