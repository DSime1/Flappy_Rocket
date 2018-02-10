using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource thruster;
    [SerializeField] float rcsThrust = 100f;
    float rotationFrame; 

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thruster = GetComponent<AudioSource>();
        rotationFrame = rcsThrust * Time.deltaTime;
       
	}
	
	// Update is called once per frame
	void Update () {

        Rotate();
        Thrusting();

	}

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            print("Collision detected"); 
        }


    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //Manually set rotation
        if (Input.GetKey(KeyCode.A))
        {
            print("rotate Left");

            transform.Rotate(Vector3.forward*rotationFrame/10);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward*rotationFrame/10);
        }
        rigidBody.freezeRotation = false; //reset natural rotation
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            if (thruster.isPlaying)
            {

                print("Playing Rocket");

            }
            else { thruster.Play(); }

            rigidBody.AddRelativeForce(Vector3.up * rotationFrame);

            print("Space Pressed");

        }
        else
        {

            thruster.Stop();
        }

    }
}
