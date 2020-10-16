using UnityEngine;

/// <summary>
/// <para>Creates the enemy</para>
/// </summary>
public class EnemyWave : MonoBehaviour, IActorTemplate
{

    // ScriptableObject values
    [SerializeField]
    int health;
    int travelSpeed;
    int fireSpeed;
    int hitPower;
    int score;

    // Wave Enemy
    [SerializeField]
    float verticalSpeed = 2;
    [SerializeField]
    float verticalAmplitude = 1;
    Vector3 sineVer;
    float time;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Attack();
    }


    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        hitPower = actorModel.hitPower;
        score = actorModel.score;
    }


    public void Die()
    {
        GameObject explode = Instantiate(Resources.Load("Prefab/explode")) as GameObject;
        explode.transform.position = this.gameObject.transform.position;
        Destroy(this.gameObject);
    }

    // Handles Enemy death and player points
    void OnTriggerEnter(Collider other)
    {
        // If hit by player or bullet
        if (other.tag == "Player")
        {
            if (health >= 1)
            {
                health -= other.GetComponent<IActorTemplate>().SendDamage();
            }
            if (health <= 0)
            {
                GameManager.Instance.GetComponent<ScoreManager>().SetScore(score);
                Die();
            }
        }
    }


    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

    public int SendDamage()
    {
        return hitPower;
    }

    /// <summary>
    /// <para>Controls the movement/attack of the Enemy</para>
    /// </summary>
    public void Attack()
    {
        time += Time.deltaTime;
        float horizontalSpeed = transform.position.x + travelSpeed * Time.deltaTime;
        sineVer.y = Mathf.Sin(time * verticalSpeed) * verticalAmplitude;
        transform.position = new Vector3(horizontalSpeed, transform.position.y + sineVer.y, transform.position.z);
    }


}
