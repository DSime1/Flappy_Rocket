﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource thruster;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThruster = 1000f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip Explosion;
    [SerializeField] AudioClip Win;

    float rotationFrame;
    float thrustFrame;

    enum State { Alive, Dying, Transcending };
    State state;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        thruster = GetComponent<AudioSource>();
        rotationFrame = rcsThrust * Time.deltaTime;
        thrustFrame = mainThruster * Time.deltaTime;

         state = State.Alive;


	}

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToRotation();
            RespondToThrustInput();
        }


	}

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            //print("Collision detected"); 

            if (state != State.Alive){

                return;
            }

            switch (collision.gameObject.tag){

                case "Friendly":
                    break;

                case "Finish":

                    StartSuccessSequence();

                    break;

                default:

                    StartDeathSequence();

                    break;
            }
        }


    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        thruster.Stop();
        thruster.PlayOneShot(Explosion);

        Invoke("LoadFirstLevel", 2f);
    }

    private void StartSuccessSequence()
    {
        print("hit Finish");
        thruster.Stop();
        thruster.PlayOneShot(Win);

        state = State.Transcending;

        Invoke("LoadNextLevel", 2f); //Invoke requires method name as a string, 1f means 1 second delay
    }

    private void LoadFirstLevel()
    {
        
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        
        SceneManager.LoadScene(1);
    }

    private void RespondToRotation()
    {
     
        rigidBody.freezeRotation = true; //Manually set rotation
        if (Input.GetKey(KeyCode.A))
        {
            //print("rotate Left");

            transform.Rotate(Vector3.forward * rotationFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationFrame);
        }
        rigidBody.freezeRotation = false; //reset natural rotation
            

      
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            
            ApplyThrust();

        }
        else
        {

            thruster.Stop();
        }


    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustFrame);
        if (!thruster.isPlaying)
        {
            thruster.PlayOneShot(mainEngine);

        }

    }
}
