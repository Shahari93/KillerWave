using UnityEngine;

public class PlayerBullet : MonoBehaviour, IActorTemplate
{
    GameObject actor;
    int hitPower;
    int health;
    int travelSpeed;
    [SerializeField] SOActorModel playerBulletSO;

    private void Awake()
    {
        ActorStats(playerBulletSO); // takes the scriptable object 
    }

    private void Update()
    {
        this.transform.position += new Vector3(travelSpeed, 0, 0) * Time.deltaTime;
    }

    public void ActorStats(SOActorModel actorModel)
    {
        hitPower = actorModel.shootPower;
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        actor = actorModel.actor;
    }

    public int SendDamage()
    {
        return hitPower; // returning how much damage this gameobkect is doing
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage; // remove amount of health from this game object according the what collided with this gameobject
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
    private void OnBecameInvisible() //This last function will remove any unnecessary bullets that have left the screen. This will help the performance of our game and keep it tidy
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.GetComponent<IActorTemplate>() != null) // checking if the object that he bullet hit has the IActorTemplate interface
            {
                if(health>=1)
                {
                    health -= other.GetComponent<IActorTemplate>().SendDamage();
                }
                if(health<=0)
                {
                    Die();
                }
            }
        }
    }
}
