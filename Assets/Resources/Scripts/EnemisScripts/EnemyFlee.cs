using UnityEngine;
using UnityEngine.AI; // In order to use the nav mesh system we need this library 

public class EnemyFlee : MonoBehaviour, IActorTemplate
{

    [SerializeField]
    SOActorModel actorModel;
    int health;
    int travelSpeed;
    int hitPower;
    int score;

    GameObject player;
    bool gameStarted = false;

    [SerializeField]
    float enemyDistanceRun = 200f; //will be used as a rule to "act" within the measured distance between the player and our fleeing enemy
    NavMeshAgent navMesh;

    private void Start()
    {
        ActorStats(actorModel);
        Invoke("DelayedStart", 0.5f);
    }

    private void DelayedStart()
    {
        gameStarted = true;
        player = GameObject.FindGameObjectWithTag("Player");
        navMesh = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // if the game is started
        if (gameStarted)
        {
            // if there is a player
            if (player != null)
            {
                // checking the distance between the enemy and the player
                float distance = Vector3.Distance(transform.position, player.transform.position);
                // if the distance is lower than 200 (the player is too close) we set the enemy a new position
                if (distance < enemyDistanceRun)
                {
                    Vector3 DirToPlayer = transform.position - player.transform.position;
                    Vector3 newPos = transform.position + DirToPlayer;
                    navMesh.SetDestination(newPos);
                }
            }

        }
    }

    public void ActorStats(SOActorModel actorModel)
    {
        navMesh.speed = actorModel.speed;
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
