using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource thruster;


	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thruster = GetComponent<AudioSource>();

       
	}
	
	// Update is called once per frame
	void Update () {

        ProcessInput();


	}

    private void ProcessInput()
    {
            if (Input.GetKey(KeyCode.Space)){
            
            if (thruster.isPlaying){

                print("Playing Rocket");

            } else {thruster.Play();}

            rigidBody.AddRelativeForce(0,(900 * Time.deltaTime),0);


            print("Space Pressed");


        }else {


            thruster.Stop(); } 


        if (Input.GetKey(KeyCode.A)){

            print("rotate Left");

            transform.Rotate(0,0,(100*Time. deltaTime));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0,(-100 * Time.deltaTime));
        }



    }
}
