
using UnityEngine;
/// <summary>
/// <para>Spawns the player at this location</para>
/// </summary>
public class PlayerSpawner : MonoBehaviour
{

    // Scriptable Object and Players ship
    SOActorModel actorModel;
    GameObject playerShip;
    bool upgradedShip = false;                     // True when there is a modified playership in the level



    // Use this for initialization
    void Start()
    {
        CreatePlayer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// <para>Creates the Player with any upgraded purchases</para>
    /// </summary>
    void CreatePlayer()
    {
        // Checks if the Player has returned from shopping
        if (GameObject.Find("UpgradedShip"))
        {
            upgradedShip = true;
        }

        // If the player hasn't gone shopping or died
        // default ship build
        if (!upgradedShip || GameManager.Instance.Died)
        {
            GameManager.Instance.Died = false;
            actorModel = Instantiate(Resources.Load("Script/ScriptableObject/Player_Default"))
                as SOActorModel;
            playerShip = Instantiate(actorModel.actor, this.transform.position, Quaternion.Euler(270, 180, 0))
                as GameObject;

            playerShip.GetComponent<IActorTemplate>().ActorStats(actorModel);
        }
        else
        {
            // Set Player ship to upgraded model
            // Adjusting the ship model
            playerShip = GameObject.Find("UpgradedShip");
        }
        // Assigning the transform of the Player ship
            playerShip.transform.rotation = Quaternion.Euler(0, 180, 0);
            playerShip.transform.localScale = new Vector3(60, 60, 60);
            playerShip.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(25, 25, 25);
            playerShip.name = "Player";
            playerShip.transform.SetParent(this.transform);
            playerShip.transform.position = Vector3.zero;
            playerShip.GetComponent<PlayerTransition>().enabled = true;             // Transition script

    }
}
