/*********************************************************************************************************************
 * 
 *  Energy Source Sript
 * 
 *  Gives the player the energy to dash
 * 
 *  -------------------------------------
 *  CHANGELOG
 *  -------------------------------------
 * Jan 16 [Ron & Austin] Simply implemented the function to recharge players dash mechanic on a trigger enter
 * to make this work attatch it to the object that you want to give the player energy
 * 
 * ********************************************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySource : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public Rigidbody rb;
    //-------------------------------------------------------------------------------------
    //
    //  StartFunction Function
    //
    //-------------------------------------------------------------------------------------
    void Start()
    {
        
    }
    //-------------------------------------------------------------------------------------
    //
    //  Update Function
    // Update is called once per frame
    //-------------------------------------------------------------------------------------

    void Update()
    {
        
    }
    //-------------------------------------------------------------------------------------
    //
    //  OnTriggerEnter Function
    //  Gets called when a trigger is entered
    //-------------------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Player.playerInstance.Regenerate();
        }
        
    }
}
