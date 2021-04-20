using UnityEngine;

public class EnemyFlee : MonoBehaviour, IActorTemplate {

    [SerializeField]
    SOActorModel actorModel;
    int health;
    int travelSpeed;
    int hitPower;
    int score;

	    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
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
	public void Die()
    {
        Destroy(this.gameObject);
    }

	    void OnTriggerEnter(Collider other)
    {
        // if the player or their bullet hits you....
        if (other.tag == "Player")
        {
            if (health >= 1)
            {
                health -= other.GetComponent<IActorTemplate>().SendDamage();    
            }
            if (health <= 0)
            {
                //died by player, apply score to 
                GameManager.Instance.GetComponent<ScoreManager>().SetScore(score);
                Die();
            }
        }
    }
}
