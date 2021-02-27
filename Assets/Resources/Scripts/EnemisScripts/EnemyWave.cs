using System;
using UnityEngine;
/// <summary>
/// Creates the wave pattern of the enemies and thier movement
/// </summary>
public class EnemyWave : MonoBehaviour, IActorTemplate
{
    int health;
    int travelSpeed;
    int fireSpeed;
    int hitPower;
    int score;

    [SerializeField] float verticalSpeed = 2f;
    [SerializeField] float verticalAmplitute = 1f;
    Vector3 sinVer;
    float time;

    private void Update()
    {
        Attack();
    }

    // Creates the wave pattern
    private void Attack()
    {
        time += Time.deltaTime;
        sinVer.y = Mathf.Sin(time * verticalSpeed) * verticalAmplitute; // creates a sin wave for the enemies movement
        transform.position = new Vector3(transform.position.x + travelSpeed * Time.deltaTime, transform.position.y + sinVer.y, transform.position.z);
    }

    // acts like a constructor of the scriptable object
    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        hitPower = actorModel.hitPower;
        score = actorModel.score;
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
        if (other.CompareTag("PlayerBullet"))
        {
            if (health >= 1)
            {
                health -= other.GetComponent<IActorTemplate>().SendDamage(); // remove health from the enemy in the amount that is set to the object that hit the enemy
            }
            if (health <= 0)
            {
                GameManager.Instance.GetComponent<ScoreManager>().SetScore(score);
                Die();
            }
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
