using System;
using UnityEngine;

public class Player : MonoBehaviour, IActorTemplate
{
    GameObject actor; // actor is the three dimensional model used to represent the player
    GameObject fire; // fire variable is the three-dimensional model of which the player fires
    GameObject _Player; // The _Player variable will be used as a reference to the _Player game object in the scene. (Empty game object that holds the player spwaner and player model)
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

    private void Update()
    {
        Movement();
        Attack();
    }

    private void Attack()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = GameObject.Instantiate(fire, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            bullet.transform.SetParent(_Player.transform);
            bullet.transform.localScale = new Vector3(7, 7, 7);
        }
    }

    private void Movement()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (transform.localPosition.x < width + width / 0.9f)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(Input.GetAxisRaw("Horizontal") * Time.deltaTime * travelSpeed, 0, 0));
            }
        }

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (transform.localPosition.x > width + width / 6f)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(Input.GetAxisRaw("Horizontal") * Time.deltaTime * travelSpeed, 0, 0));
            }
        }

        if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (transform.localPosition.y > -height / 3f)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(0, Input.GetAxisRaw("Vertical") * Time.deltaTime * travelSpeed, 0));
            }
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (transform.localPosition.y < height / 2.5f)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(0, Input.GetAxisRaw("Vertical") * Time.deltaTime * travelSpeed, 0));
            }
        }
    }

    //The code we have just entered assigns values from the player's SOActorModel ScriptableObject asset we made earlier
    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        hitPower = actorModel.shootPower;
        travelSpeed = actorModel.speed;
        fire = actorModel.actorBullets;
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

    public int SendDamage()
    {
        return hitPower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (health >= 1)
            {
                if (transform.Find("energy +1(Clone)")) // checks to see whether the collider has a game object named energy + 1(Clone). (The name of this object is the name of the shield the player can purchase in the game shop)
                {
                    Destroy(transform.Find("energy + 1(Clone)").gameObject);
                    health -= other.GetComponent<IActorTemplate>().SendDamage();
                }
                else
                {
                    health -= 1;
                }
            }
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
