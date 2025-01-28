using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 acceleration;
    [SerializeField] private float maxSpeed;
    private float currentMaxSpeed;
    [SerializeField] private float friction;
    [SerializeField] private float counterAccelerationModifier;
    
    [SerializeField] private float accelerationSpeed;
    private float currentAccelerationSpeed;
    [SerializeField] private List<Transform> wallList = new List<Transform>();
    [SerializeField] private float movementIncrement;
    
    [SerializeField] private float driftingForce;
    [SerializeField] private float turboAcclerationForce;
    [SerializeField] private float turboMaxSpeed;
    [SerializeField] private GameObject turboBoostBar;
    private float maxTurboBoostCharge = 100f;
    private float currentTurboBoostCharge = 0f;


    private void Start()
    {
      Application.targetFrameRate = 60;
      turboBoostBar.GetComponent<Image>().fillAmount = 0f;
      currentAccelerationSpeed = accelerationSpeed;
      currentMaxSpeed = maxSpeed;
    }


    void Update()
    {
        // Reset the acceleration to zero
        acceleration = Vector3.zero;
        
        // Set acceleration based on player pressing WASD
        if (Input.GetKey(KeyCode.W)) acceleration.y += 1;
        if (Input.GetKey(KeyCode.S)) acceleration.y -= 1;
        if (Input.GetKey(KeyCode.D)) acceleration.x += 1;
        if (Input.GetKey(KeyCode.A)) acceleration.x -= 1;


        // Normalize the acceleration and multiply it by the accelerationSpeed and deltaTime
        acceleration = acceleration.normalized * (currentAccelerationSpeed * Time.deltaTime);
        
        // Calculate the angle between the velocity and the acceleration
        float angleVelocityVsAcceleration = Vector3.Angle(velocity, acceleration);
        // counterPushRatio is the 0 to 1 version of angleVelocityVsAcceleration
        float counterPushRatio = angleVelocityVsAcceleration / 180;


        // Add the acceleration to the velocity, and add a bit more acceleration based on how much the player is trying to counter the velocity
        velocity += acceleration + (acceleration * (counterPushRatio * counterAccelerationModifier));
        
        // Drifting logic
        if (Input.GetKey(KeyCode.A) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W)))
        {
            velocity *= driftingForce;
            if (currentTurboBoostCharge <= maxTurboBoostCharge) currentTurboBoostCharge += 0.5f;
            else if (currentTurboBoostCharge > maxTurboBoostCharge) currentTurboBoostCharge = maxTurboBoostCharge;
            turboBoostBar.GetComponent<Image>().fillAmount = currentTurboBoostCharge / maxTurboBoostCharge;
        }
        
        // Turbo Boost logic
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.D))
        {
            if (currentTurboBoostCharge > 0.0f)
            {
                currentAccelerationSpeed *= turboAcclerationForce;
                currentMaxSpeed *= turboMaxSpeed;
                currentTurboBoostCharge -= 3.0f;
            }
            else if (currentTurboBoostCharge < 0.0f) currentTurboBoostCharge = 0.0f;
            turboBoostBar.GetComponent<Image>().fillAmount = currentTurboBoostCharge / maxTurboBoostCharge;
        }
        else
        {
            currentAccelerationSpeed = accelerationSpeed;
            currentMaxSpeed = maxSpeed;
        }
        
        // If the velocity is greater than maxSpeed, set the velocity to maxSpeed
        if (velocity.magnitude > (currentMaxSpeed * Time.deltaTime)) velocity = velocity.normalized * (currentMaxSpeed * Time.deltaTime);
        
        // If the player is not pressing any keys, apply friction to the velocity
        if (acceleration == Vector3.zero)
        {
            if (velocity.magnitude < friction * Time.deltaTime) velocity = Vector3.zero;
            else
            {
                velocity -= velocity.normalized * (friction * Time.deltaTime);
            }
        }


        // In the section below we move the player by tiny increments of the x and y velocities until the player touches a wall or until we have used the entire velocity for this frame
        Vector3 velocityThisFrame = velocity; //how much velocity we have left to use this frame
        Vector3 microVelocity; //the tiny increment of velocity we will use in each loop of the while loop
        Vector3 positionNextFrame = transform.position; //the position the player will be in after we have used all of the velocity for this frame
        bool velocityXPositive = false;
        if (velocityThisFrame.x > 0) velocityXPositive = true;
        bool velocityYPositive = false;
        if (velocityThisFrame.y > 0) velocityYPositive = true;


        while (velocityThisFrame != Vector3.zero) //keep looping through this until we have used all of the velocity for this frame (or we touch a wall)
        {
            if (Mathf.Abs(velocityThisFrame.x) > Mathf.Abs(velocityThisFrame.y)) //if x movement magnitude is greater than y movement magnitude
            {
                velocityThisFrame.x -= MovementIncrementSigned(velocityXPositive); //subtract the movement increment we are using up from the velocity x
                microVelocity = new Vector3(MovementIncrementSigned(velocityXPositive), 0, 0); //set the microVelocity's x to the movement increment we are using up
                if (IsThisTransformInAWall(positionNextFrame + microVelocity)) //if I would be touching a wall
                {
                    velocityThisFrame.x = 0; //no more velocity left this frame (because we hit the wall)
                }
                else //if I would not be touching a wall
                {
                    positionNextFrame += microVelocity; //move the player by the microVelocity
                }
                
                //if the velocity we have left is less than a movementIncrement, set velocityThisFrame.x to 0
                if (velocityThisFrame.x < movementIncrement && velocityThisFrame.x > -movementIncrement)
                {
                    velocityThisFrame.x = 0;
                }
            }
            //The section below is the same as the x movement section, but for y movement.
            //Check the comments above if you need to understand what's being done here.
            else //if velocityThisFrame y > velocity this frame x. 
            {
                velocityThisFrame.y -= MovementIncrementSigned(velocityYPositive);
                microVelocity = new Vector3(0, MovementIncrementSigned(velocityYPositive), 0);
                if (IsThisTransformInAWall(positionNextFrame + microVelocity)) //if I would be touching a wall
                {
                    velocityThisFrame.y = 0;
                }
                else //if I would not be touching a wall
                {
                    positionNextFrame += microVelocity;
                }


                if (velocityThisFrame.y < movementIncrement && velocityThisFrame.y > -movementIncrement)
                {
                    velocityThisFrame.y = 0;
                }
            }
        }
        //finally, move the player by positionNextFrame
        transform.position = positionNextFrame;
    }


    bool IsThisTransformInAWall(Vector3 positionToCheck)
    {
        //for each wall in the wallList
        foreach (Transform currentWall in wallList)
        {
            // Calculate the distance between the player and the wall in X and Y
            float xDistance = Mathf.Abs(positionToCheck.x - currentWall.position.x);
            float yDistance = Mathf.Abs(positionToCheck.y - currentWall.position.y);
            // Find the maximum distance in X and Y by adding half of the player's scale and half of the wall's scale
            float xMaxDistance = transform.localScale.x / 2 + currentWall.localScale.x / 2;
            float yMaxDistance = transform.localScale.y / 2 + currentWall.localScale.y / 2;


            // If the player is closer to the wall than the maximum distance (half player scale + half wall scale) in X and in Y, return true
            if (xDistance < xMaxDistance && yDistance < yMaxDistance) return true;
        }
        // If the player is not closer to any wall than the maximum distance, return false
        return false;
    }


    /// <summary>
    /// Returns the movement increment if positiveDirection is true, otherwise returns the negative of the movement increment
    /// </summary>
    /// <param name="positiveDirection"></param>
    /// <returns></returns>
    float MovementIncrementSigned(bool positiveDirection)
    {
        if (positiveDirection) return movementIncrement;
        else return -movementIncrement;
    }
}