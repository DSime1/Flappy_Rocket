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
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip Explosion;
    [SerializeField] AudioClip Win;
    [SerializeField] float loadLevelDelay = 2f;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem ExplosionParticles;
    [SerializeField] ParticleSystem WinParticles;


    bool CollisionDisable;
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

        CollisionDisable = false;


	}

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToRotation();
            RespondToThrustInput();
        }

        //works only when is not development build (when I am coding)
        if (Debug.isDebugBuild)
        {
            RespondToDebugMode();
        }
    }

    private void RespondToDebugMode()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (CollisionDisable)
            {
                CollisionDisable = false;
            }
                else 
            {
                CollisionDisable = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (state != State.Alive)
            {
                return;
            }

            switch (collision.gameObject.tag){

                case "Friendly":
                    break;

                case "Finish":

                    StartSuccessSequence();

                    break;

                default:
                    if (CollisionDisable)
                    {
                        return;
                    }
                    else
                    {
                        StartDeathSequence();
                    }
                    break;
            }
        }


    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        thruster.Stop();
        mainEngineParticles.Stop();
        thruster.PlayOneShot(Explosion);
        ExplosionParticles.Play();

        Invoke("LoadFirstLevel", loadLevelDelay);
    }

    private void StartSuccessSequence()
    {
        print("hit Finish");
        thruster.Stop();
        mainEngineParticles.Stop();
        thruster.PlayOneShot(Win);
        WinParticles.Play();

        state = State.Transcending;

        Invoke("LoadNextLevel", loadLevelDelay); //Invoke requires method name as a string, 1f means 1 second delay
    }

    private void LoadFirstLevel()
    {
        
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        int currentScene;
        int nextScene;

        currentScene = SceneManager.GetActiveScene().buildIndex;
        nextScene = ++currentScene;

        if (nextScene == SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(0);

        }
        else
        {
            SceneManager.LoadScene(nextScene);

        }
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
            mainEngineParticles.Stop();
        }


    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * thrustFrame);
        if (!thruster.isPlaying)
        {
            thruster.PlayOneShot(mainEngine);
            mainEngineParticles.Play();

        }

    }
}
