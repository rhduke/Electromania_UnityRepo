﻿/*********************************************************************************************************************
 * 
 *  Player Script
 * 
 *  Controls the player unit.
 * 
 *  -------------------------------------
 *  CHANGELOG
 *  -------------------------------------
 *  Jan 21[Ron & Austin] - Made a static instance of the player class to use for easier access. Combined the player and energy class
 *  because there was no point to having both classes static
 *  
 *  Jan 20 [Ron] - Added and modified some comments for submission.
 *  
 *  Jan 17 [Ron] - Changed is dashing from a bool to a function that check to see if the time is between 0 and the max
 *  dash time. Cleaned up the code in the update functiont to make it more readable. Adjusted deceleration implementation
 *  by using a bool and starting it after a percentage of the full dash time. It is still buggy as it seems to be affected
 *  by how far you click from the player. Deceleration is commented out for now.
 *  
 *  Jan 16v2 [Ron & Austin] - Added fixed Z position took out the canDash bool and made a class for having the energy for
 *  access and usage. Added deceleration for the the dash so it doesn't keep on moving in a projectile motion. Added 
 *  turning off gravity during the dash for a more dash like affect. dash is buggy sometimes does a super dash
 *  
 *  Jan 16 [Ron & Austin] - Created OnGUI() test for debugging. Changed the vector coords for the dash to use 
 *  Camera.WorldToScreenInput rather than Camera.ScreenToWorldInput so it will associate the player and the mouse with 
 *  screen coordinates. Converting to world input was not working properly. Added timer, turned off gravity
 *  
 *  Jan 15 [Nero] - Created FixedUpdate() and Dash() functions.
 * 
 * ********************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------------------------------------------------------
//  PLAYER CLASS
//-----------------------------------------------------------------------------------------

public class  Player : MonoBehaviour
{
    public static Player playerInstance;
    //-------------------------------------------------------------------------------------
    //  CONST Variables
    //-------------------------------------------------------------------------------------

    
    public const float NOT_DASHING = -1.0f;        // dashTimer is set to this value when hes not dashing
    public const float DASH_ACC_SCALER = 0.8f;     // Used to determine when to start decelerating during dash
    public const float Z_POSITION = -1.5f;         // Constant z position of player

    //-------------------------------------------------------------------------------------
    //  Public Variables
    //-------------------------------------------------------------------------------------

    public Camera cam;          
    public Rigidbody rb;

    public int health;
    public float jumpForce;
    public float moveSpeed;
    public float dashForce;
    public float dashDuration;          // Total time of the dash
    public float decelerationForce; // Amount of deceleration
    public float decelerateTime;    // The time at which to start decelerating during the dash
    public bool canDash;

    public Vector3 inputs;

    //-------------------------------------------------------------------------------------
    //  Private Variables
    //-------------------------------------------------------------------------------------

    private Vector3 plyrScreenPos;   // Position of the player in screen coordinates
    private float dashTimer;         // Current time elapsed in the dash
    private bool isDecelerating;     // Used to check to see if plyr is currently decelerating during dash

    //-------------------------------------------------------------------------------------
    //  Start Function
    //-------------------------------------------------------------------------------------
    public void Start()
    {
        canDash = true;
        playerInstance = this;
        health = 0;
        jumpForce = 0.0f;
        moveSpeed = 5.0f;                // Ideal speed
        dashForce = 60.0f;               // Ideal force
        dashDuration = 0.1f;                 // Ideal time
        decelerationForce = 50;          // Ideal deceleration
        decelerateTime = dashDuration * DASH_ACC_SCALER;    // For example, ply will start decelerating after going through 80% of the dash
        dashTimer = NOT_DASHING;
        isDecelerating = false;
    }

    //-------------------------------------------------------------------------------------
    //  FixedUpdate Function
    //
    //  Called every frame and updates the position of the player object. Calls the dash
    //  function when the LMB is clicked.
    //-------------------------------------------------------------------------------------
    public void FixedUpdate()
    {
        plyrScreenPos = cam.WorldToScreenPoint(transform.position);     // Converts the player position to screen coordinates

        HorizontalMoveListener();   // Move player left/right if left/right arrows or A/D keys pressed
        DashListener();             // Calls dash function when LMB is pressed

        transform.position = new Vector3(transform.position.x, transform.position.y, Z_POSITION);   // Keep player's z coordinate constant
    }

    //-------------------------------------------------------------------------------------
    //  Dash Function
    //
    //  Dash works by determining the resulting vector between the LMB click position and
    //  the 
    //-------------------------------------------------------------------------------------
    public void DashListener()
    {
        //---------------------------------------------------------------------------------
        // Dash
        // - Add the horizontal force on the player when the LMB is pressed and he currently isn't dashing
        if (Input.GetMouseButtonDown(0) && canDash && !IsDashing())
        {
            rb.velocity = Vector3.zero;                                                     // Reset velocity to zero
            rb.AddForce((Input.mousePosition - plyrScreenPos).normalized * dashForce, ForceMode.VelocityChange);     // Add the force to change the velocity of the player
            rb.useGravity = false;              // Remove gravity while dashing
            StartDashTimer();
        }

        //---------------------------------------------------------------------------------
        // Decelerate (IN DEVELOPMENT STILL)
        // - Start decelerating at a certain time 
        //if (!isDecelerating && dashTimer >= decelerateTime)
        //{
        //    isDecelerating = true;
        //    rb.AddForce(((Input.mousePosition - plyrScreenPos).normalized * -1 * decelerationForce), ForceMode.Acceleration);
        //}

        //---------------------------------------------------------------------------------
        // Timer
        // Increment dash time and reset timer and other vars when completed
        if (IsDashing())
        {
            dashTimer += Time.deltaTime;        // Increment timer

            if (IsDashComplete())               // Once the dash is completed, reset settings on player
            {
                rb.useGravity = true;
                isDecelerating = false;
                dashTimer = NOT_DASHING;
                rb.velocity = Vector3.zero;
                //Energy.Discharge();
            }
        }
    }
    
    //-------------------------------------------------------------------------------------
    //  Horizontal Key Press Function
    //-------------------------------------------------------------------------------------
    public void HorizontalMoveListener()
    {
        inputs.x = Input.GetAxis("Horizontal");     // Used for getting the keyboard inputs from the player (A, D and left and right arrows)

        // Moves the player when the left and right directional keys are pressed
        if (inputs.x != 0)
        {
            rb.MovePosition(transform.position + inputs * moveSpeed * Time.deltaTime);
        }
    }

    //-------------------------------------------------------------------------------------
    //  Is Dashing Function
    //-------------------------------------------------------------------------------------
    public bool IsDashing()
    {
        return dashTimer >= 0 && dashTimer < dashDuration;
    }

    //-------------------------------------------------------------------------------------
    //  Is Dash Complete Function
    //-------------------------------------------------------------------------------------
    public bool IsDashComplete()
    {
        return dashTimer >= dashDuration;
    }

    //-------------------------------------------------------------------------------------
    //  Reset Dash Timer Function
    //-------------------------------------------------------------------------------------
    public void StartDashTimer()
    {
        dashTimer = 0;
    }
    //-------------------------------------------------------------------------------------
    //
    //  Regenerate Function
    //
    //-------------------------------------------------------------------------------------
    public void Regenerate()                          //Function that regains energy setting canDash to true
    {
        canDash = true;
    }
    //-------------------------------------------------------------------------------------
    //
    //  Discharge Function
    //
    //-------------------------------------------------------------------------------------
    public  void Discharge()                            //Function thatloses energy setting canDash to false
    {
        canDash = false;
    }
    //-------------------------------------------------------------------------------------
    //  OnGUI Function
    //-------------------------------------------------------------------------------------
    private void OnGUI()
    {
        GUI.Label(new Rect(30, 30, 500, 200), "Mouse screen location: " + Input.mousePosition.x + ", " + Input.mousePosition.y);                            // Output coordates of the mouse location
        GUI.Label(new Rect(30, 50, 500, 200), "Obj world position: " + transform.position.x + ", " + transform.position.y + ", " + transform.position.z);   // Output coordinates of player world position
        GUI.Label(new Rect(30, 70, 500, 200), "Obj screen position: " + plyrScreenPos.x + ", " + plyrScreenPos.y);                                          // Output coordinates of player screen position
        GUI.Label(new Rect(30, 90, 500, 200), "Obj velocity " + rb.velocity.x + ", " + rb.velocity.y);                                                      // Output player velocity
        GUI.Label(new Rect(400, 30, 500, 200), "Dash Timer: " + dashTimer);                                     // Dash timer
        GUI.Label(new Rect(400, 50, 500, 200), "Dash Duration: " + dashDuration);
        GUI.Label(new Rect(200, 170, 500, 200), "Use 'A' and 'D' to move left and right");                       // Movement
        GUI.Label(new Rect(200, 200, 500, 200), "Use Left mouse click to dash");                                 // Dashing
    }
}