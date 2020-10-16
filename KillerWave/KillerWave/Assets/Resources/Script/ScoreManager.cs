using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// <para>Stores the Players Score information</para>
/// </summary>
public class ScoreManager : MonoBehaviour {

	public static int playerScore;
	public int PlayersScore
    {
        get { return playerScore; }
    }


	/// <summary>
	/// <para>Adds to the Players score</para>
	/// </summary>
	/// <param name="incomingScore">Score to add</param>
	public void SetScore(int incomingScore)
    {
		playerScore += incomingScore;
        if (GameObject.Find("score"))
        {
            GameObject.Find("score").GetComponent<Text>().text = playerScore.ToString();
        }
		Debug.Log("Score: "+ playerScore);
    }

	public void ResetScore()
    {
		playerScore = 00000000;
        if (GameObject.Find("score"))
        {
			GameObject.Find("score").GetComponent<Text>().text = playerScore.ToString();
        }
    }

}
