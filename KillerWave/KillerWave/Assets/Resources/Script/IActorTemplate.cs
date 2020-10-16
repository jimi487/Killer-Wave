/// <summary>
/// <para>Interface to prompt damage control, death, and scriptable objects</para>
/// <br>Inherited by Player, PlayerBullet, PlayerSpawner, Enemy, EnemyBullet, EnemySpawner</br>
/// </summary>
public interface IActorTemplate
{

    int SendDamage();                                   // Amount of Damage to deal
    void TakeDamage(int incomingDamage);                // Amount of Damage to take away
    void Die();                                         // Handling removing the Object
    void ActorStats(SOActorModel actorModel);           // Handles Initializing the object from SO

}
