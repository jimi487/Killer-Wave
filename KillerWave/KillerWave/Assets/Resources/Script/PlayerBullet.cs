using UnityEngine;

/// <summary>
/// <para>Script for the Player's Bullet</para>
/// </summary>
public class PlayerBullet : MonoBehaviour, IActorTemplate
{

    GameObject actor;
    int hitPower;
    int health;
    int travelSpeed;

    [SerializeField]
    SOActorModel bulletModel;

    /// <summary>
    /// <para>Creates the Bullet from the bullet Scritpable Object</para>
    /// </summary>
    /// <param name="actorModel"></param>
    public void ActorStats(SOActorModel actorModel)
    {
        hitPower = actorModel.hitPower;
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        actor = actorModel.actor;
    }


    void Awake()
    {
        ActorStats(bulletModel);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(travelSpeed, 0, 0) * Time.deltaTime;
    }


    public void Die()
    {
        Destroy(this.gameObject);
    }

    /// <summary>
    /// <para>Damage done when colliding</para>
    /// </summary>
    /// <returns></returns>
    public int SendDamage()
    {
        return hitPower;
    }


    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<IActorTemplate>() != null)
            {
                if (health >= 1)
                {
                    health -= other.GetComponent<IActorTemplate>().SendDamage();
                }
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    /// <summary>
    /// Destroying the bullets that have left the screen
    /// </summary>
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
