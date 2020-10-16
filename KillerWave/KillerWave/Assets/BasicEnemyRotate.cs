using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Gives the Enemy Ring some animation</para>
/// </summary>
public class BasicEnemyRotate : MonoBehaviour {

	[SerializeField]
	float speed = 200;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.left * Time.deltaTime * speed);
	}
}
