using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour, IActorTemplate
{

    // SOActorModel variables
    int travelSpeed;
    int health;
    int hitPower;           // How much damage done when collidiing
    GameObject actor;       // Player GameObject
    GameObject fire;

    float camTravelSpeed;   // Sets the pace of the players ship when camera moves on x
    public float CamTravelSpeed
    {
        get { return camTravelSpeed; }
        set { camTravelSpeed = value; }
    }
    float movingScreen;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public GameObject Fire
    {
        get { return fire; }
        set { fire = value; }
    }

    GameObject _Player;     // _Player GameObject in Hierarchy for organization
    GameObject[] screenPoints = new GameObject[2];
    
    // World Space dimensions of the Game Screen
    //float width;
    //float height;

    Vector3 direction;      // Holds players touchscreen location
    Rigidbody rb;
    public static bool mobile = false;



    // Use this for initialization
    void Start()
    {
        // Creating the Viewport Points 
        //height = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);
        //width = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);

        // _Player GameObject in Scene
        _Player = GameObject.Find("_Player");
        //movingScreen = width;

        mobile = false;
#if UNITY_ANDROID && !UNITY_EDITOR
mobile = true;
InvokeRepeating("Attack", 0, 0.3f);
rb = GetComponent<RigidBody>();
#endif
        CalculateBoundaries();
    }

    // Update is called once per frame
    void Update() {
        if (Time.timeScale == 1) 
        { 
            PlayersSpeedWithCamera();
            if (mobile) 
            { 
                MobileControls(); 
            } 
            else 
            { 
                Movement();
                Attack(); 
            }
        } 
    
    }
    /// <summary>
    /// <para>Sets up the Player stats from SOActorModel</para>
    /// </summary>
    /// <param name="actorModel">The Scriptable Object containing the Player information</param>
    public void ActorStats(SOActorModel actorModel)
    {
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        hitPower = actorModel.hitPower;
        fire = actorModel.actorsBullets;
    }

    /// <summary>
    /// Handles Player interacting with the enemy
    /// </summary>
    /// <param name="other">The other collider</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (health >= 1)
            {
                // Destroys the Shield around player
                if (transform.Find("energy +1(Clone)"))
                {
                    Destroy(transform.Find("energy +1(Clone)").gameObject);
                    health -= other.GetComponent<IActorTemplate>().SendDamage();
                }
            else
            {
                health -= 1;
                }

            }
        }
        if (health <= 0)
        {
            Die();
        }
    }
    /// <summary>
    /// <para>Damages the players Health</para>
    /// </summary>
    /// <param name="incomingDamage">Damage being received</param>
    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

    /// <summary>
    /// <para>Damage dealt when colliding with player</para>
    /// </summary>
    /// <returns></returns>
    public int SendDamage()
    {
        return hitPower;
    }

    /// <summary>
    /// <para>Handles the User Movement and restricts the Player within bounds</para>
    /// </summary>
    void Movement()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float horizontalSpeed = horizontalInput * Time.deltaTime * travelSpeed;
        float verticalInput = Input.GetAxisRaw("Vertical");
        float verticalSpeed = verticalInput * Time.deltaTime * travelSpeed;

        // Horizontal Movement
        if (horizontalInput > 0)
        {
            if (transform.localPosition.x < 
                (screenPoints[1].transform.localPosition.x - screenPoints[1].transform.localPosition.x/30f)+movingScreen)
            {
                transform.localPosition += new Vector3(horizontalSpeed, 0, 0);
            }
        }
        if (horizontalInput < 0)
        {
            if (transform.localPosition.x <
    (screenPoints[1].transform.localPosition.x - screenPoints[1].transform.localPosition.x / 30f) + movingScreen)
            {
                transform.localPosition += new Vector3(horizontalSpeed, 0, 0);
            }
        }

        // Vertical Movement
        if (verticalInput < 0)
        {
            // Checking if player is above the Shopview view
            if (transform.localPosition.y >
                (screenPoints[1].transform.localPosition.y -  
                    screenPoints[1].transform.localPosition.y / 3f))
            {
                transform.localPosition += new Vector3(0, verticalSpeed, 0);
            }
        }
        if (verticalInput > 0)
        {
            if (transform.localPosition.y <
                (screenPoints[0].transform.localPosition.y -  
                   screenPoints[0].transform.localPosition.y / 5f))
            {
                transform.localPosition += new Vector3(0, verticalSpeed, 0);
            }
        }


    }

    /// <summary>
    /// <para>Destroys the Player</para>
    /// </summary>
    public void Die()
    {
        GameObject explode = Instantiate(Resources.Load("Prefab/explode")) as GameObject;
        explode.transform.position = this.gameObject.transform.position;
        GameManager.Instance.LifeLost();
        Destroy(this.gameObject);
    }

    /// <summary>
    /// <para>Fires a bullet from the Players location</para>
    /// </summary>
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") || mobile)
        {
            GameObject bullet = Instantiate(Fire, transform.position,
                Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
            bullet.transform.SetParent(_Player.transform);
            bullet.transform.localScale = new Vector3(7, 7, 7);
        }
    }

    /// <summary>
    /// <para>Handles camera moving with the player</para>
    /// </summary>
    void PlayersSpeedWithCamera()
    {
        if (camTravelSpeed > 1) 
        { 
            transform.position += Vector3.right * Time.deltaTime * camTravelSpeed; 
            movingScreen += Time.deltaTime * camTravelSpeed;
        }
        else
        {
            movingScreen = 0;
        }
    }

    /// <summary>
    /// <para>Handles mobile control inputs</para>
    /// </summary>
    void MobileControls() { 
        if (Input.touchCount > 0 && EventSystem.current.currentSelectedGameObject == null) 
        { 
            Touch touch = Input.GetTouch(0); 
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 300)); 
            touchPosition.z = 0; 
            direction = (touchPosition - transform.position); 
            rb.velocity = new Vector3(direction.x, direction.y, 0) * 5; 
            direction.x += movingScreen; 
            if (touch.phase == TouchPhase.Ended)
            { 
                rb.velocity = Vector3.zero; 
            } 
        } 
    }

    /// <summary>
    /// <para>Calculates the screen boundaries</para>
    /// </summary>
    void CalculateBoundaries()
    {
        screenPoints[0] = new GameObject("p1");
        screenPoints[1] = new GameObject("p2");
        Vector3 v1 = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 300));
        Vector3 v2 = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 300));
        screenPoints[0].transform.position = v1;
        screenPoints[1].transform.position = v2;
        screenPoints[0].transform.SetParent(this.transform.parent);
        screenPoints[1].transform.SetParent(this.transform.parent);
        movingScreen = screenPoints[1].transform.position.x;
    }

}
