
using System.Collections;
using UnityEngine;
using UnityEngine.Monetization;

/// <summary>
/// <para>Handles Player Interaction in the Shop</para>
/// <br>Creates a new Ship and adds purchased power ups to it</br>
/// </summary>
public class PlayerShipBuild_OLD : MonoBehaviour 
{
	// Shop Variables
	[SerializeField]
	GameObject[] shopButtons;
	GameObject target;
 	GameObject tmpSelection;	
	GameObject textBoxPanel;	

	// Ad Variables
	string placementId_rewardedvideo = "rewardedVideo";
  	string gameId = "1234567";

	// Weapon and ship Variables
	[SerializeField]
	GameObject[] visualWeapons;
	[SerializeField]
	SOActorModel defaultPlayerShip;
	GameObject playerShip;
	
	// Bank Variables
	GameObject buyButton;
	GameObject bankObj;
	int bank = 600;
	bool purchaseMade = false;


	void Start()
	{
		// Resets the purchase
		purchaseMade = false;
		bankObj = GameObject.Find("bank");
		bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
		textBoxPanel = GameObject.Find("textBoxPanel");
		buyButton = textBoxPanel.transform.Find("BUY ?").gameObject;
		CheckPlatform();
		TurnOffPlayerShipVisuals();
		PreparePlayerShipForUpgrade();
		TurnOffSelectionHighlights();
	}
	
	/// <summary>
	/// <para>Determines what platform the player is on</para>
	/// </summary>
	void CheckPlatform()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			gameId = "123456";  //CHANGE TO YOUR ID
		}
		else if (Application.platform == RuntimePlatform.Android)
		{
			gameId = "123456";  //CHANGE TO YOUR ID
		}
		Monetization.Initialize(gameId,false);
	}

	/// <summary>
	/// <para>Creates a Player ship game object to apply upgrades to</para>
	/// <br>Sets its position above the screen out of the Players view</br>
	/// </summary>
	void PreparePlayerShipForUpgrade()
	{
	 	playerShip = Instantiate(Resources.Load("Prefab/Player/Player_Ship")) as GameObject;
	 	playerShip.GetComponent<Player>().enabled = false;
	 	playerShip.transform.position = new Vector3(0,10000,0);
	 	playerShip.GetComponent<IActorTemplate>().ActorStats(defaultPlayerShip);
	}

	/// <summary>
	/// <para>Handles Player selecting an item on the Selection screen</para>
	/// </summary>
	/// <param name="hit"></param>
	/// <returns></returns>
	GameObject ReturnClickedObject (out RaycastHit hit)
	{
		GameObject target = null;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		
		if (Physics.Raycast (ray.origin, ray.direction * 100, out hit)) 
		{
			target = hit.collider.gameObject;
		}
		return target;
	}

	/// <summary>
	/// <para>Turns off the selection highlights of the GameObjects in menu</para>
	/// </summary>
	void TurnOffSelectionHighlights()
	{
		for (int i = 0; i < shopButtons.Length; i++)
		{
			shopButtons[i].SetActive(false);
		}
	}

	/// <summary>
	/// <para>Updates the Description box</para>
	/// </summary>
	void UpdateDescriptionBox()
	{
		textBoxPanel.transform.Find("name").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().ShopSelection.iconName;
		textBoxPanel.transform.Find("desc").gameObject.GetComponent<TextMesh>().text = tmpSelection.GetComponentInParent<ShopPiece>().ShopSelection.description;	
	}

	/// <summary>
	/// <para>Sets the box display to active</para>
	/// </summary>
	void Select()
	{
		tmpSelection = target.transform.Find("SelectionQuad").gameObject;
		tmpSelection.SetActive(true);
	}

	/// <summary>
	/// <para>Attempts to Select a purchasable item</para>
	/// </summary>
	public void AttemptSelection()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			RaycastHit hitInfo;
			target = ReturnClickedObject (out hitInfo);

			if (target != null)
		
			{
					if (target.transform.Find("itemText"))
					{ 
						TurnOffSelectionHighlights();
						Select();
						UpdateDescriptionBox();
						

						//NOT ALREADY SOLD
						if (target.transform.Find("itemText").GetComponent<TextMesh>().text != "SOLD")
						{
							//can afford
							Affordable();

							//can not afford
							LackOfCredits();
						}
						else if (target.transform.Find("itemText").GetComponent<TextMesh>().text == "SOLD")
						{
							SoldOut();
						}
					}
		   			else if(target.name == "BUY ?")
        			{
         		 		BuyItem();
       		 		}
     				else if (target.name == "WATCH AD")
					{
						WatchAdvert();
					}
					    else if(target.name == "START")
					{
						StartGame();
					}
				 }
			}
		}
		
		void WatchAdvert()
		 {
			if (Application.internetReachability != NetworkReachability.NotReachable)
			{
			  ShowRewardedAds();
			}   
		 }
        
		IEnumerator WaitForAd () 
		{
			string placementId = placementId_rewardedvideo;
			while (!Monetization.IsReady (placementId)) 
			{
				yield return null;
			}

			ShowAdPlacementContent ad = null;
			ad = Monetization.GetPlacementContent (placementId) as ShowAdPlacementContent;

			if (ad != null) 
			{
				ad.Show (AdFinished);          
			}
		}
		
	    void AdFinished (ShowResult result)
		{
			if (result == ShowResult.Finished) 
			{
			  bank += 300;
			  bankObj.GetComponentInChildren<TextMesh>().text = bank.ToString();
			  TurnOffSelectionHighlights(); 
			}
		}
		
	  void ShowRewardedAds()
	  {
		StartCoroutine (WaitForAd());
	  }
	  void BuyItem()
	  {
		Debug.Log("PURCHASED");
		purchaseMade = true;
		buyButton.SetActive(false);
		tmpSelection.SetActive(false);
		for (int i = 0; i < visualWeapons.Length; i++)
		{
		if (visualWeapons[i].name == tmpSelection.transform.parent.gameObject.
										GetComponent<ShopPiece>().ShopSelection.iconName)
			{
				visualWeapons[i].SetActive(true);
			}
		}
		UpgradeToShip(tmpSelection.transform.parent.gameObject.GetComponent<ShopPiece>().ShopSelection.iconName);

		bank = bank - int.Parse(tmpSelection.transform.parent.GetComponent<ShopPiece>().ShopSelection.cost);
		
		bankObj.transform.Find("bankText").GetComponent<TextMesh>().text = bank.ToString();
		tmpSelection.transform.parent.transform.Find("itemText").GetComponent<TextMesh>().text = "SOLD";
	  }

	/// <summary>
	/// <para>Assigns the upgrade to the Ship</para>
	/// </summary>
	/// <param name="upgrade">The upgrade prefab as a string</param>
    void UpgradeToShip(string upgrade)
	{
		GameObject shipItem = Instantiate(Resources.Load("Prefab/Player/"+upgrade)) as GameObject;
		shipItem.transform.SetParent(playerShip.transform);
		shipItem.transform.localPosition = Vector3.zero; 
	}


	void Update()
	{
		AttemptSelection();
	}


	void Affordable()
	{
		if (bank >= int.Parse(target.transform.GetComponent<ShopPiece>().ShopSelection.cost))
		{
		  Debug.Log("CAN BUY");
		  buyButton.SetActive(true);
		}
	}

    void LackOfCredits()
	{
		if (bank < System.Int32.Parse(target.transform.Find("itemText").GetComponent<TextMesh>().text))
		{
			Debug.Log("CAN'T BUY");
		}
	}

  	void SoldOut()
	{
		Debug.Log("SOLD OUT");
	}

	void TurnOffPlayerShipVisuals()
	{
		for (int i = 0; i < visualWeapons.Length; i++)
		{
		  visualWeapons[i].gameObject.SetActive(false);
		}
	}
	
	/// <summary>
	/// <para>Starts the game</para>
	/// </summary>
	void StartGame()
	{	 
		if (purchaseMade)
    {
      playerShip.name = "UpgradedShip";
      
	  if (playerShip.transform.Find("energy +1(Clone)"))
      {
        playerShip.GetComponent<Player>().Health = 2;
      } 
      DontDestroyOnLoad(playerShip);
    }
		GameManager.Instance.GetComponent<ScenesManager>().BeginGame(GameManager.gameLevelScene);
  }
}
