using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    static GameManager instance;                // GameManager Singleton instance
    public static int currentScene = 0;         // Current Scene index
    public static int gameLevelScene = 3;       // Holds the first level we play
    public static int playerLives = 3;
    bool died = false;
    public bool Died
    {
        get { return died; }
        set { died = value; }
    }

    public static GameManager Instance
    {
        get { return instance; }
    }


    void Awake()
    {
        CheckGameManagerIsInScene();
        currentScene = SceneManager.GetActiveScene().buildIndex;
        LightandCameraSetup(currentScene);
    }


    // Use this for initialization
    void Start()
    {
        SetLivesDisplay(playerLives);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// <para>Sets up the Camera at the start of the game</para>
    /// </summary>
    void CameraSetup(float camSpeed)
    {
        GameObject gameCamera = GameObject.FindGameObjectWithTag("MainCamera");
        gameCamera.GetComponent<CameraMovement>().CamSpeed = camSpeed;

        // Setting the Camera's Transform
        gameCamera.transform.position = new Vector3(0, 0, -300);
        gameCamera.transform.eulerAngles = new Vector3(0, 0, 0);

        // Setting the Camera's Properties
        gameCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        gameCamera.GetComponent<Camera>().backgroundColor = new Color32(0, 0, 0, 255);
    }
    /// <summary>
    /// <para>Sets up the Directional Light at the start of the game</para>
    /// <br>Sets its rotation and its lighting</br>
    /// </summary>
    void LightSetup()
    {
        GameObject dirLight = GameObject.Find("Directional Light");
        dirLight.transform.eulerAngles = new Vector3(50, -30, 0);
        dirLight.GetComponent<Light>().color = new Color32(152, 204, 255, 255);
    }

    /// <summary>
    /// <para>Creates Singleton GameManager GameObject in Hierarchy</para>
    /// </summary>
    void CheckGameManagerIsInScene()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// <para>Sets up the Lights and Camera for the Scene</para>
    /// </summary>
    /// <param name="sceneNumber">Scene to set up</param>
    void LightandCameraSetup(int sceneNumber)
    {
        switch (sceneNumber)
        {
            // testScene, level1, level2, level3
            case 3:
            case 4:
                {
                    LightSetup();
                    CameraSetup(0);
                    break;
                }
            case 5:
                {
                    CameraSetup(150);
                    break;
                }
        }
    }

    /// <summary>
    /// <para>Handles Player losing a life</para>
    /// </summary>
    public void LifeLost()
    {
        StartCoroutine(DelayedLifeLost()); 
    }

    /// <summary>
    /// <para>Updates the UI to display the players lives</para>
    /// </summary>
    /// <param name="players"></param>
    public void SetLivesDisplay(int players)
    {
        if (GameObject.Find("lives"))
        {
            GameObject lives = GameObject.Find("lives");
            if (lives.transform.childCount < 1)
            {
                // Creating Life prefab
                for (int i = 0; i < 5; i++)
                {
                    GameObject life = Instantiate(Resources.Load("Prefab/life")) as GameObject;
                    life.transform.SetParent(lives.transform);
                }
            }
                // Scaling the transform
                for(int i = 0; i < lives.transform.childCount; i++)
                {
                    lives.transform.GetChild(i).localScale = new Vector3(1, 1, 1);
                }

                // Remove visual lives
                for(int i = 0; i < (lives.transform.childCount - players); i++)
                {
                    lives.transform.GetChild(lives.transform.childCount - i - 1).localScale = Vector3.zero;
                }
            }
    }

    IEnumerator DelayedLifeLost()
    {
        yield return new WaitForSeconds(2);
        if (playerLives >= 1)
        {
            playerLives--;
            Debug.Log("Lives left: " + playerLives);
            GetComponent<ScenesManager>().ResetScene();
        }
        else
        {
            GetComponent<ScenesManager>().GameOver();
        }
    }

}
