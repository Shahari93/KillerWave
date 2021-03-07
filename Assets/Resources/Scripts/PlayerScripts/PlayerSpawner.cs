﻿using System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    SOActorModel actorModel;
    GameObject playerShip;
    private bool isUpgradedShip = false;

    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        GetComponentInChildren<Player>().enabled = true;
    }

    private void CreatePlayer()
    {
        //been shopping for upgrades
        if (GameObject.Find("Upgraded Ship"))
        {
            isUpgradedShip = true;
        }
        if (!isUpgradedShip || GameManager.Instance.Died)
        {
            GameManager.Instance.Died = false;
            actorModel = GameObject.Instantiate(Resources.Load("Scripts/ScriptableObject/Player_Default")) as SOActorModel; // instantiate the player ship ScriptableObject asset and store it in the actorModel variable
            playerShip = GameObject.Instantiate(actorModel.actor, this.transform.position, Quaternion.Euler(270, 180, 0)) as GameObject;
            playerShip.GetComponent<IActorTemplate>().ActorStats(actorModel);
        }
        else
        {
            playerShip = GameObject.Find("Upgraded Ship");
        }
        playerShip.transform.rotation = Quaternion.Euler(0, 180, 0);
        playerShip.transform.localScale = new Vector3(60, 60, 60);
        playerShip.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(25, 25, 25);
        playerShip.name = "Player";
        playerShip.transform.SetParent(this.transform);
        playerShip.transform.position = Vector3.zero;
    }
}
//// Create the player
//actorModel = GameObject.Instantiate(Resources.Load("Scripts/ScriptableObject/Player_Default")) as SOActorModel; // instantiate the player ship ScriptableObject asset and store it in the actorModel variable
//playerShip = GameObject.Instantiate(actorModel.actor) as GameObject; // instantiate a game object that refers to our ScriptableObject that holds the game object called actor in our object variable named playerShip
//playerShip.GetComponent<Player>().ActorStats(actorModel); // we apply our ScriptableObject asset to the playerShip method called ActorStats that exists in the Player component script

//// Setup the player
//playerShip.transform.rotation = Quaternion.Euler(0, 180, 0); // Set the starting rotation of the player gameobject
//playerShip.transform.localScale = new Vector3(60, 60, 60); // Set the starting size of the player gameobject
//playerShip.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(25, 25, 25); //accesses the player's ship's ParticleSystem component and changes its scale to 25 on all axes
//playerShip.name = "Player"; // Set the name of the game object
//playerShip.transform.SetParent(this.transform); // Set the player game object to be a child of the empty game object (_Player)
//playerShip.transform.position = Vector3.zero; // Set the position to start at vector 0
