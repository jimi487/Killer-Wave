using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{

    Scenes scenes;
    public enum Scenes
    {
        bootUp,
        title,
        shop,
        level1,
        level2,
        level3,
        gameOver
    }

    float gameTimer = 0;                                    // How long the level has until over
    float[] endLevelTimer = { 30, 30, 45 };
    int currentSceneNumber = 0;
    bool gameEnding = false;

    public MusicMode musicMode;
    public enum MusicMode
    {
        noSound, fadeDown, musicOn
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));         // Starts music
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        // Checks for the current scene
        if (currentSceneNumber != SceneManager.GetActiveScene().buildIndex)
        {
            currentSceneNumber = SceneManager.GetActiveScene().buildIndex;
            GetScene();
        }
        GameTimer();
    }

    /// <summary>
    /// <para>Adjusts the music volume</para>
    /// </summary>
    /// <param name="musicMode">noSound, fadeDown, musicOn</param>
    /// <returns></returns>
    IEnumerator MusicVolume(MusicMode musicMode)
    {
        switch (musicMode)
        {
            // Stops the sound
            case MusicMode.noSound:
                {
                    GetComponentInChildren<AudioSource>().Stop();
                    break;
                }
            // Fades the volume down
            case MusicMode.fadeDown:
                {
                    GetComponentInChildren<AudioSource>().volume -= Time.deltaTime / 3;
                    break;
                }
            // Plays music
            case MusicMode.musicOn:
                {
                    if (GetComponentInChildren<AudioSource>().clip != null)
                    {
                        GetComponentInChildren<AudioSource>().Play();
                        GetComponentInChildren<AudioSource>().volume = 1;
                    }
                    break;
                }
        }               // Giving time for game to change settings
        yield return new WaitForSeconds(0.1f);
    }



    /// <summary>
    /// <para>Resets the Scene when Player loses life but hasn't died</para>
    /// </summary>
    public void ResetScene()
    {
        StartCoroutine(MusicVolume(MusicMode.noSound));
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene);
    }

    /// <summary>
    /// <para>Moves the Player to the gameOver Scene when the game ends</para>
    /// </summary>
    public void GameOver()
    {
        SceneManager.LoadScene("gameOver");
        Debug.Log("ENDSCORE: " + GameManager.Instance.GetComponent<ScoreManager>().PlayersScore);
    }

    /// <summary>
    /// <para>Starts the game with the testScene Scene</para>
    /// </summary>
    public void BeginGame(int gameLevel)
    {
        gameTimer = 0;
        SceneManager.LoadScene(gameLevel);
    }

    /// <summary>
    /// <para>Advances the scene</para>
    /// </summary>
    void NextLevel()
    {
        gameEnding = false;
        gameTimer = 0;
        SceneManager.LoadScene(GameManager.currentScene + 1);
        StartCoroutine(MusicVolume(MusicMode.musicOn));
    }


    /// <summary>
    /// <para>Initiates the Scenes variable</para>
    /// </summary>
    void GetScene()
    {
        scenes = (Scenes)currentSceneNumber;
    }

    /// <summary>
    /// <para>Timer for the Game</para>
    /// <br>Adds time to the clock and checks if its reached timer limit</br>
    /// </summary>
    void GameTimer()
    {
        switch (scenes)
        {
            // Checking what the current scene is
            case Scenes.level1:
            case Scenes.level2:
            case Scenes.level3:
                {
                    // Playing Music
                    if (GetComponentInChildren<AudioSource>().clip == null)
                    {
                        AudioClip lvlMusic = Resources.Load<AudioClip>("Sound/lvlMusic") as AudioClip;
                        GetComponentInChildren<AudioSource>().clip = lvlMusic;
                        GetComponentInChildren<AudioSource>().Play();
                    }

                    if (gameTimer < endLevelTimer[currentSceneNumber - 3])
                    {
                        // Level has not completed
                        gameTimer += Time.deltaTime;
                    }
                    else
                    {
                        StartCoroutine(MusicVolume(MusicMode.fadeDown));
                        // Level is complete
                        if (!gameEnding && GameObject.Find("Player"))
                        {
                            gameEnding = true;

                            // Stops player controls
                            GameObject player = GameObject.Find("Player");
                            player.GetComponent<Player>().enabled = false;
                            player.GetComponent<Rigidbody>().isKinematic = true;
                            Player.mobile = false;
                            CancelInvoke();

                            if (SceneManager.GetActiveScene().name != "level3")
                            {
                                player.GetComponent<PlayerTransition>().LevelEnds = true;
                            }
                            // Final level
                            else
                            {
                                player.GetComponent<PlayerTransition>().GameCompleted = true;
                            }

                            SendInJsonFormat(SceneManager.GetActiveScene().name);
                            Invoke("NextLevel", 4);
                        }
                    }
                    break;
                }
            default:
                {
                    GetComponentInChildren<AudioSource>().clip = null;            // Resets audio clip
                    break;
                }

        }
    }



    /// <summary>
    /// <para>Sets the lives in the UI when the scene loads</para>
    /// </summary>
    /// <param name="Scene"></param>
    /// <param name="aMode"></param>
    private void OnSceneLoaded(Scene Scene, LoadSceneMode aMode)
    {
        StartCoroutine(MusicVolume(MusicMode.musicOn));
        GetComponent<GameManager>().SetLivesDisplay(GameManager.playerLives);
        if (GameObject.Find("score"))
        {
            GameObject.Find("score").GetComponent<Text>().text = ScoreManager.playerScore.ToString();
        }
    }

    /// <summary>
    /// <para>Converts to JSON format</para>
    /// </summary>
    /// <param name="lastLevel"></param>
    void SendInJsonFormat(string lastLevel)
    {
        if (lastLevel == "level3")
        {
            GameStats gameStats = new GameStats();
            gameStats.livesLeft = GameManager.playerLives;
            gameStats.completed = System.DateTime.Now.ToString();
            gameStats.score = ScoreManager.playerScore;

            string json = JsonUtility.ToJson(gameStats, true);
            Debug.Log(json);
            Debug.Log(Application.persistentDataPath + "/GameStatsSaved.json");
            System.IO.File.WriteAllText(Application.persistentDataPath + "/GameStatsSaved.json", json);


        }
    }

}
