using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Gives the player an extra lives when using remote settings</para>
/// </summary>
public class RemoteSettingsStartUp : MonoBehaviour {

	void Awake()
    {
		if(Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork ||
			Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
			RemoteSettings.Updated += () =>
			{
				GameManager.playerLives = RemoteSettings.GetInt("PlayersStartUpLives", GameManager.playerLives);
			};
        }
    }




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
