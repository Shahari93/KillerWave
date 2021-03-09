using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour
{
    #region setup
    Vector3 transitionToEnd = new Vector3(-100, 0, 0); // coordinates to set our player at the start of the level
    Vector3 transitionToCompleteGame = new Vector3(7000, 0, 0); // Used when the player completes level 3 and we change the exit animation

    // Check the distance from where the player is and where he needs to travel
    Vector3 readyPos = new Vector3(900, 0, 0);
    Vector3 startPos;

    float distCovered; // hold time data that will be used to measure 2 vector3 points
    float journeyLength; // will hold distance between startPos and readyPos

    bool levelStarted = true;
    bool speedOff = false; // will be set to true when we want the ship to leave the screen
    bool levelEnd = false; // will be set to true when the level has come to end
    bool gameCompleted = false; // will be set to true when the whole game will be completed

    // we will be using those 2 properties to get access to the 2 bools from other scripts
    bool LevelEnd
    {
        get
        {
            return levelEnd;
        }

        set
        {
            levelEnd = value;
        }
    }

    bool GameCompleted
    {
        get
        {
            return gameCompleted;
        }
        set
        {
            gameCompleted = value;
        }
    }
    #endregion

    private void Start()
    {
        this.transform.localPosition = Vector3.zero; // reset the player ship to the playerspawner gameobkect
        startPos = transform.position;
        Distance();
    }

    private void Distance()
    {
        journeyLength = Vector3.Distance(startPos, readyPos); // measure the distance between two vector points, and the answer is in a form of a float
    }

    private void Update()
    {
        if (levelStarted)
        {
            StartCoroutine(PlayerMovement(transitionToEnd, 10));
        }
        if (levelEnd)
        {
            GetComponent<Player>().enabled = false;
            GetComponent<SphereCollider>().enabled = false; // turning the collider off in case of enemy or enemy bullet hits the player
            Distance();
            StartCoroutine(PlayerMovement(transitionToEnd, 200));
        }
        if (gameCompleted)
        {
            GetComponent<Player>().enabled = false;
            GetComponent<SphereCollider>().enabled = false; // turning the collider off in case of enemy or enemy bullet hits the player
            StartCoroutine(PlayerMovement(transitionToCompleteGame, 200));
        }
        if (speedOff)
        {
            Invoke("SpeedOff", 1f);
        }
    }

    //animating our player ship in the near center of the screen in order to begin and also exit the level
    IEnumerator PlayerMovement(Vector3 point, float transitionSpeed)
    {
        // check to see if the player is on the right place
        if (Mathf.Round(transform.localPosition.x) >= readyPos.x - 5 && Mathf.Round(transform.localPosition.x) <= readyPos.x + 5 && Mathf.Round(transform.localPosition.y) >= -5 && Mathf.Round(transform.localPosition.y) <= 5)
        {
            if (levelEnd)
            {
                levelEnd = false;
                speedOff = true;
            }
            if (levelStarted)
            {
                levelStarted = false;
                distCovered = 0f;
                GetComponent<Player>().enabled = true;
            }
        }
        else
        {
            distCovered += Time.deltaTime * transitionSpeed; // checks what the distance that the player traveled 
            float fractionOfJourney = distCovered / journeyLength; // what percantage of the journey the player passed
            transform.position = Vector3.Lerp(transform.position, point, fractionOfJourney); // linearly interpolates our player's ship between two points
        }
        // we wait frame for instructions to be applied
        yield return null;
    }

    void SpeedOff()
    {
        transform.Translate(Vector3.left * Time.deltaTime * 800);
    }
}
