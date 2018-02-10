using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource thruster;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThruster = 1000f;
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
            Rotate();
            Thrusting();
        }

        if (state != State.Alive)
        {

            thruster.Stop();
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
                    print("ok");
                    break;
                case "Finish":
                    print("hit Finish");
                    state = State.Transcending;
                    Invoke("LoadNextLevel", 2f); //Invoke requires method name as a string, 1f means 1 second delay

                    break;

                default:
                    print("u dead");

                    state = State.Dying;

                    Invoke("LoadFirstLevel", 2f);


                    break;


            }
        }


    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void Rotate()
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
