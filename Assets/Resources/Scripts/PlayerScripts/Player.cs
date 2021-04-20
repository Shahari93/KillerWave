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


    float camTravelSpeed;
    public float CamTravelSpeed
    {
        get
        {
            return camTravelSpeed;
        }
        set
        {
            camTravelSpeed = value;
        }
    }
    float moveingScreen; // will hold the result of Time.deltatime multiplied by camTravelspeed


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
        moveingScreen = width;
    }

    private void Update()
    {
        //check to see if the game's timeScale is running at full speed(1) and then carries on with the Movement and Attack methods.
        if (Time.timeScale == 1f)
        {
            Movement();
            Attack();
        }
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = GameObject.Instantiate(fire, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            bullet.transform.SetParent(_Player.transform);
            bullet.transform.localScale = new Vector3(7, 7, 7);
        }
    }

    private void Movement()
    {

        if (camTravelSpeed > 1)
        {
            //we increment the player's ship's X-axis to the right multiplied by Time.deltatime and camTravelSpeed.
            transform.position += Vector3.right * camTravelSpeed * Time.deltaTime;
            moveingScreen += Time.deltaTime * camTravelSpeed;
        }

        float horMove = Input.GetAxisRaw("Horizontal");
        float verMove = Input.GetAxisRaw("Vertical");
        if (horMove > 0)
        {
            if (transform.localPosition.x < moveingScreen + (width / 0.9f))
            {
                transform.localPosition += Vector3.Normalize(new Vector3(horMove * Time.deltaTime * travelSpeed, 0, 0));
            }
        }

        if (horMove < 0)
        {
            if (transform.localPosition.x > moveingScreen + width / 6f)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(horMove * Time.deltaTime * travelSpeed, 0, 0));
            }
        }

        if (verMove < 0)
        {
            if (transform.localPosition.y > -height / 3f)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(0, verMove * Time.deltaTime * travelSpeed, 0));
            }
        }

        if (verMove > 0)
        {
            if (transform.localPosition.y < height / 2.5f)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(0, verMove * Time.deltaTime * travelSpeed, 0));
            }
        }
    }

    //The code we have just entered assigns values from the player's SOActorModel ScriptableObject asset we made earlier
    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        hitPower = actorModel.hitPower;
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
                    Destroy(transform.Find("energy +1(Clone)").gameObject);
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
        GameManager.Instance.LiveLost(); //we can call the GameManager script directly without finding the game object in the scene
        Destroy(this.gameObject);
    }
}
