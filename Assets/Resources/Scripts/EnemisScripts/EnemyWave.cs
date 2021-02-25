using System;
using UnityEngine;

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

    private void Attack()
    {
        time += Time.deltaTime;
        sinVer.y = Mathf.Sin(time * verticalSpeed) * verticalAmplitute;
        transform.position = new Vector3(transform.position.x + travelSpeed * Time.deltaTime, transform.position.y + sinVer.y, transform.position.z);
    }

    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        hitPower = actorModel.shootPower;
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
                Debug.Log("End Score: " + GameManager.Instance.GetComponent<ScoreManager>().PlayerScore);
                Die();
            }
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
