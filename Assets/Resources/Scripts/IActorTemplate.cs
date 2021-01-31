using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// our interface will be used by 6 classes that share the same behavior but we will implement those behaviors in a different way for each class
/// </summary>
public interface IActorTemplate 
{
    int SendDamage(); // a method that sends damage that the player/enemy are doing
    void TakeDamage(int incomingDamage); // method for damage that the player/enemy are getting
    void Die(); // a death method
    void ActorStats(SOActorModel actorModel); // a method that hold the ScriptableObject data
}
