/*********************************************************************************************************************
 * 
 *  Energy script
 * 
 *  Controls the players ability to dash
 * 
 *  -------------------------------------
 *  CHANGELOG
 *  -------------------------------------
 * 
 * Jan 16 [Ron & Austin] Added member canDash seperated it from the playerdash class for easier access as well
 * as made setter and getter functions for canDash
 * 
 * ********************************************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Energy
{
    // Start is called before the first frame update
    public static bool canDash;
    //-------------------------------------------------------------------------------------
    //
    //  Energy Class Constructor
    //
    //-------------------------------------------------------------------------------------
    static Energy()
    {
        canDash = true;   
        //inits candash to true
    }
    //-------------------------------------------------------------------------------------
    //
    //  Regenerate Function
    //
    //-------------------------------------------------------------------------------------
    public static void Regenerate()                          //Function that regains energy setting canDash to true
    {
        canDash = true;
    }
    //-------------------------------------------------------------------------------------
    //
    //  Discharge Function
    //
    //-------------------------------------------------------------------------------------
    public static void Discharge()                            //Function thatloses energy setting canDash to false
    {
        canDash = false;
    }
    //-------------------------------------------------------------------------------------
    //
    //  canIDash Function
    //
    //-------------------------------------------------------------------------------------
    public static bool canIDash()                               //getter function to see if the player can dash
    {
        return canDash;
    }
}
