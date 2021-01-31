using UnityEngine;
/// <summary>
/// This scriptable object will be connected to the player, the player bullet and the enemies. Each of them share most of the same data so it's best in unity to do it with scriptable object
/// </summary>


[CreateAssetMenu(fileName = "Create Actor", menuName = "Create Actor")] //The CreateAssetMenu attribute creates an extra selection from the drop-down list in the Project window in the Unity editor when we right-click and select Create
public class SOActorModel : ScriptableObject
{
    public string actorName;
    public string actorDescription;
    public int health;
    public int speed;
    public int shootPower;
    public GameObject actor;
    public GameObject actorBullets;
    public AttackType attackType;
    public enum AttackType { wave, flee, bullet, player };
}
