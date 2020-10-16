using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>ScriptableObject as a template for Ship upgrades in the shop</para>
/// </summary>
[CreateAssetMenu(fileName = "Create Shop Piece", menuName = "Create Shop Piece")]
public class SOShopSelection : ScriptableObject {

	// SO Variables
	public Sprite icon;				// Picture of the selection
	public string iconName;			// Identifies what the selection is
	public string description;		// Describes what the upgrade is in the large selection board
	public string cost;				// How many credits worth


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
