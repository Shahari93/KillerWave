using UnityEngine;

public class Player : MonoBehaviour, IActorTemplate
{
    GameObject actor; // actor is the threedimensional model used to represent the player
    GameObject fire; // fire variable is the three-dimensional model of which the player fires
    GameObject _Player; // The _Player variable will be used as a reference to the _Player game object in the scene.
    int travelSpeed;
    int hitPower;
    int health;

    //The last two variables of width and height will be used to store the
    //measured results of the world space dimensions of the screen the game is
    //played in.
    float height;
    float width;

    //The two public properties of Health and Fire are there to give access to
    //our two private health and fire variables from other classes that
    //require access.
    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }
    public GameObject Fire
    {
        get
        {
            return fire;
        }
        set
        {
            fire = value;
        }
    }

    private void Start()
    {
        //viewport space is similar to what we know as a screen resolution, except its measurements, are in points and not pixels, and these points are measured from 0 to 1.
        height = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - .5f); // WorldToViewportPoint method take the results from the game's three-dimensional world space and convert the results into viewport space
        width = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f); // This will give us our current world space width of the screen.
        _Player = GameObject.Find("_Player");
    }

    //private void Update()
    //{
    //    Movement();
    //    Attack();
    //}


    //The code we have just entered assigns values from the player's SOActorModel ScriptableObject asset we made earlier
    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        hitPower = actorModel.shootPower;
        travelSpeed = actorModel.speed;
        fire = actorModel.actorBullets;
    }



    public int SendDamage()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(int incomingDamage)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
