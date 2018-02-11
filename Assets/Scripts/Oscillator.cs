using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Prevents to add 2 scripts to same object
[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour {

    GameObject redObstacles;

    [SerializeField] float period = 2f;

    [SerializeField] Vector3 movementVector;

    [Range(0, 1)] float movementFactor;

    Vector3 startingPos;
    Vector3 offset;

	// Use this for initialization
	void Start () {

        movementVector = new Vector3 (0, 4, 0);

        redObstacles = GameObject.Find("MovingObstacles");

        startingPos = redObstacles.transform.position ;		
	}
	
	// Update is called once per frame
	void Update () {

       

        //prevent NaN when inserting period = zero.
        if (period <= Mathf.Epsilon)
        {
            print("enter valid Period");
            return;
        }


        float cycles = Time.time / period;

        const float tau =  Mathf.PI * 2f;

        float rawSinWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSinWave/2f + 0.5f;

        offset = movementVector * movementFactor;

        //movementVector = startingPos;
        redObstacles.transform.position = startingPos + offset;

	}
}
