﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IActorTemplate
{
    GameObject actor; // actor is the three dimensional model used to represent the player
    GameObject fire; // fire variable is the three-dimensional model of which the player fires
    GameObject _Player; // The _Player variable will be used as a reference to the _Player game object in the scene. (Empty game object that holds the player spwaner and player model)
    int travelSpeed;
    int hitPower;
    int health;
    Vector3 direction; //will hold the player's touch screen location
    Rigidbody rb;
    public static bool mobile = false;
    GameObject[] screenPoints = new GameObject[2];
    //The last two variables of width and height will be used to store the
    //measured results of the world space dimensions of the screen the game is
    //played in.
    //float height;
    //float width;


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
        mobile = false;
#if UNITY_ANDROID && !UNITY_EDITOR
mobile = true;
InvokeRepeating("Attack",0,0.3f);
rb = GetComponent<Rigidbody>();
#endif
        _Player = GameObject.Find("_Player");
        CalculateBoundaries();
        //viewport space is similar to what we know as a screen resolution, except its measurements, are in points and not pixels, and these points are measured from 0 to 1.
        //height = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - .5f); // WorldToViewportPoint method take the results from the game's three-dimensional world space and convert the results into viewport space
        //width = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f); // This will give us our current world space width of the screen.
        //moveingScreen = width;
    }

    private void CalculateBoundaries()
    {
        // Creates 2 new gameobjects named p1 and p2
        screenPoints[0] = new GameObject("p1");
        screenPoints[1] = new GameObject("p2");

        //ViewportToWorldPoint give us our game world space position
        Vector3 v1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 300));
        Vector3 v2 = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 300));

        //apply our new Vector3 variables, v1 and v2, to our array of game object's positions
        screenPoints[0].transform.position = v1;
        screenPoints[1].transform.position = v2;

        //need to make them children of the Player script, which will update their Transform Position values.
        screenPoints[0].transform.SetParent(this.transform.parent);
        screenPoints[1].transform.SetParent(this.transform.parent);

        //we update the movingScreen float value with our screenPoint value for when the game has a moving camera.
        moveingScreen = screenPoints[1].transform.position.x;
    }

    private void Update()
    {
        //check to see if the game's timeScale is running at full speed(1) and then carries on with the Movement and Attack methods.
        if (Time.timeScale == 1f)
        {
            PlayerSpeedWithCamera();
            if (mobile)
            {
                MobileControl();
            }
            else
            {
                Movement();
                Attack();
            }
        }
    }

    private void MobileControl()
    {
        // Checking if there is a touch on screen, and that we didn't touched on any of UI stuff
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 300));
            touchPos.z = 0;
            direction = (touchPos - transform.position);
            rb.velocity = new Vector3(direction.x, direction.y, 0) * 5;
            direction.x += moveingScreen;
            if (touch.phase == TouchPhase.Ended)
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    private void PlayerSpeedWithCamera()
    {
        if (camTravelSpeed > 1)
        {
            //we increment the player's ship's X-axis to the right multiplied by Time.deltatime and camTravelSpeed.
            transform.position += Vector3.right * camTravelSpeed * Time.deltaTime;
            moveingScreen += Time.deltaTime * camTravelSpeed;
        }
        else
            moveingScreen = 0;
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Fire1") || mobile)
        {
            GameObject bullet = GameObject.Instantiate(fire, this.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;
            bullet.transform.SetParent(_Player.transform);
            bullet.transform.localScale = new Vector3(7, 7, 7);
        }
    }

    private void Movement()
    {
        float horMove = Input.GetAxisRaw("Horizontal");
        float verMove = Input.GetAxisRaw("Vertical");
        if (horMove > 0)
        {
            if (transform.localPosition.x < (screenPoints[1].transform.localPosition.x - screenPoints[1].transform.localPosition.x / 30f) + moveingScreen)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(horMove * Time.deltaTime * travelSpeed, 0, 0));
            }
        }

        if (horMove < 0)
        {
            if (transform.localPosition.x > (screenPoints[1].transform.localPosition.x - screenPoints[1].transform.localPosition.x / 30) + moveingScreen)
            {
                transform.localPosition += Vector3.Normalize(new Vector3(horMove * Time.deltaTime * travelSpeed, 0, 0));
            }
        }

        if (verMove < 0)
        {
            if (transform.localPosition.y > (screenPoints[1].transform.localPosition.y - screenPoints[1].transform.localPosition.y / 3f))
            {
                transform.localPosition += Vector3.Normalize(new Vector3(0, verMove * Time.deltaTime * travelSpeed, 0));
            }
        }

        if (verMove > 0)
        {
            if (transform.localPosition.y < (screenPoints[0].transform.localPosition.y - screenPoints[0].transform.localPosition.y / 5f))
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
        GameObject explode = Instantiate(Resources.Load("Prefab/explode")) as GameObject;
        explode.transform.position = this.gameObject.transform.position;
        GameManager.Instance.LiveLost(); //we can call the GameManager script directly without finding the game object in the scene
        Destroy(this.gameObject);
    }
}
