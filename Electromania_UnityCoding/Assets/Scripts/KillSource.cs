/*********************************************************************************************************************
 * 
 *  Kill Source Script
 * 
 *  Give this script to an object to kill the player on contact
 * 
 *  -------------------------------------
 *  CHANGELOG
 *  -------------------------------------
 *  
 * 
 * ********************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSource : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //-------------------------------------------------------------------------------------
    //  On Trigger Enter Function
    //-------------------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        // Need to differentiate between capsules and enemies

        if (other.CompareTag("Battery"))
        {
            Player.playerInstance.Regenerate();
        }

    }
}
