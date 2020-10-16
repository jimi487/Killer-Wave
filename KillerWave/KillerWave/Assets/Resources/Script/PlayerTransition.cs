using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransition : MonoBehaviour {

	Vector3 transitionToEnd = new Vector3(-100, 0, 0);			// Where Player transitions to at the statr of level
	Vector3 transitionToCompleteGame = new Vector3(7000, 0, 0);	// Used when player finishes final level
	Vector3 readyPos = new Vector3(900, 0, 0);
	Vector3 startPos;
	float distCovered;			// Time used to calculate distance between vector3 points
	float journeyLength;        // Holds the distance between the vector3 points

	bool levelStarted = true;
	bool speedOff = false;
	bool levelEnds = false;
	bool gameCompleted = false;

	public bool LevelEnds
    {
        get { return levelEnds; }
        set { levelEnds = value; }
    }

	public bool GameCompleted
    {
        get { return gameCompleted; }
        set { gameCompleted = value; }
    }

	// Use this for initialization
	void Start () {
		// Positioning the Player to parent Game Object
		this.transform.localPosition = Vector3.zero;
		startPos = transform.position;
		Distance();
	}
	
	// Update is called once per frame
	void Update () {
        if (levelStarted)
        {
			StartCoroutine(PlayerMovement(transitionToEnd, 10));
        }

        if (levelEnds)
        {
			GetComponent<Player>().enabled = false;
			GetComponent<SphereCollider>().enabled = false;
			Distance();
			StartCoroutine(PlayerMovement(transitionToEnd, 200));
        }

        if (gameCompleted)
        {
			GetComponent<Player>().enabled = false;
			GetComponent<SphereCollider>().enabled = false;
			StartCoroutine(PlayerMovement(transitionToCompleteGame, 200));
        }

        if (speedOff)
        {
			Invoke("SpeedOff", 1f);
        }

	}

	void Distance()
    {
		journeyLength = Vector3.Distance(startPos, readyPos);
    }


	/// <summary>
	/// <para>Animates the Player Ship near the center of screen to begin and exit level</para>
	/// </summary>
	/// <param name="point"></param>
	/// <param name="transitionSpeed"></param>
	/// <returns></returns>
	IEnumerator PlayerMovement(Vector3 point, float transitionSpeed)
    {
		// Determining whether the Player is in the right position
		if(Mathf.Round(transform.localPosition.x) >= readyPos.x -5 && 
			Mathf.Round(transform.localPosition.x) <= readyPos.x +5 &&
			Mathf.Round(transform.localPosition.y) >= -5f &&
			Mathf.Round(transform.localPosition.y) <= +5f)
        {
			// Apply the speed off animation when leve ends
            if(levelEnds)
			{
				levelEnds = false;
				speedOff = true;
            }

			// Returns player Movement after animation ends
            if (levelStarted)
            {
				levelStarted = false;
				distCovered = 0;
				GetComponent<Player>().enabled = true;
            }
			yield return null;
        }
        else
        {
			distCovered += Time.deltaTime * transitionSpeed;
			float fractionOfJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(transform.position, point, fractionOfJourney);
        }
    }

	/// <summary>
	/// <para>Makes Player jet off screen when level ends</para>
	/// </summary>
	void SpeedOff()
    {
		transform.Translate(Vector3.left * Time.deltaTime * 800);
    }

}
