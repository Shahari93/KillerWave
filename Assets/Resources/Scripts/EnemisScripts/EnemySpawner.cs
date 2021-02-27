using System;
using System.Collections;
using UnityEngine;
// TODO: Try to refactor the EnemySpawner class to use the prototype design pattern

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] SOActorModel soActorModel = null;
    [SerializeField] float spawnRate = 0;
    [SerializeField] [Range(1, 10)] int quantity = 1;
    GameObject enemies;

    private void Awake()
    {
        enemies = GameObject.Find("_Enemies");
        StartCoroutine(FireEnemies(quantity, spawnRate));
    }

    private IEnumerator FireEnemies(int quantity, float spawnRate)
    {
        for (int i = 0; i < quantity; i++)
        {
            GameObject enemyUnit = CreateEnemy();
            enemyUnit.gameObject.transform.SetParent(this.transform); // setting the spawned enemy parent gameobject to be the _Enemies gameobject
            enemyUnit.transform.position = this.transform.position;
            yield return new WaitForSeconds(spawnRate); // the coroutine will wait until the spawnRate will be less or equal to zero, until spawning enemies again
        }
        yield return null;
    }

    private GameObject CreateEnemy()
    {
        GameObject enemy = GameObject.Instantiate(soActorModel.actor) as GameObject; //Instantiate the enemy game object from its ScriptableObject asset
        enemy.GetComponent<IActorTemplate>().ActorStats(soActorModel); //Apply values to our enemy from its ScriptableObject asset.
        enemy.name = soActorModel.actorName.ToString(); //Name the enemy game object from its ScriptableObject asset.
        return enemy;
    }
}
