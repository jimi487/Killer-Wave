using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    SOActorModel actorModel;
    [SerializeField]
    float spawnRate;
    [SerializeField]
    [Range(0, 10)]
    int quantity;
    GameObject enemies;


    void Awake()
    {
        enemies = GameObject.Find("_Enemies");
        StartCoroutine(FireEnemy(quantity, spawnRate));
    }

    /// <summary>
    /// <para>Creates ans positions the Enemy</para>
    /// </summary>
    /// <param name="qty">How many enemies to spawn</param>
    /// <param name="spwnRte">How long to wait before spawning another</param>
    /// <returns></returns>
    IEnumerator FireEnemy(int qty, float spwnRte)
    {
        for (int i = 0; i < qty; i++)
        {
            GameObject enemyUnit = CreateEnemy();
            enemyUnit.gameObject.transform.SetParent(this.transform);
            enemyUnit.transform.position = transform.position;
            yield return new WaitForSeconds(spwnRte);
        }
        yield return null;
    }


    GameObject CreateEnemy()
    {
        GameObject enemy = Instantiate(actorModel.actor) as GameObject;
        enemy.GetComponent<IActorTemplate>().ActorStats(actorModel);
        enemy.name = actorModel.actorName.ToString();
        return enemy;
    }
}
