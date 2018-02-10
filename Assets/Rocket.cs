using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource thruster;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThruster = 1000f;
    float rotationFrame;
    float thrustFrame;


	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thruster = GetComponent<AudioSource>();
        rotationFrame = rcsThrust * Time.deltaTime;
        thrustFrame = mainThruster * Time.deltaTime;
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
            //print("Collision detected"); 

            switch (collision.gameObject.tag){

                case "Friendly":
                    print("ok");
                    break;

                default:
                    print("u dead");
                    break;


            }
        }


    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //Manually set rotation
        if (Input.GetKey(KeyCode.A))
        {
            //print("rotate Left");

            transform.Rotate(Vector3.forward*rotationFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward*rotationFrame);
        }
        rigidBody.freezeRotation = false; //reset natural rotation
    }

    private void Thrusting()
    {
        if (Input.GetKey(KeyCode.Space))
        {

            if (thruster.isPlaying)
            {

             //   print("Playing Rocket");

            }
            else { thruster.Play(); }

            rigidBody.AddRelativeForce(Vector3.up * thrustFrame);

           // print("Space Pressed");

        }
        else
        {

            thruster.Stop();
        }

    }
}
