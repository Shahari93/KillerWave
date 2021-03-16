using System;
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
        playerShip.GetComponent<PlayerTransition>().enabled = true; // This line of code will make our player ship animate into the scene.
    }
}
