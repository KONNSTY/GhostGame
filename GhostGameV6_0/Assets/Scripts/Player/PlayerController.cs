using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float horizontal;
    public float vertical;
    public float speed;
    public float injuryspeed; // Speed when injured

    public Rigidbody rb;

    public Camera cameraVC;

    public FixedJoystick joystick;

    public Canvas canvas;
    public Image healthBar;

    public Image enegeryBar;

    public Animator animator;
    public GameObject Character;

    [HideInInspector] public Vector3 dir;

    public bool hasInunity = false; // Flag to check if player has infinity energy

    public float healthPlus = 20f;

    public int Maxhealthitem = 3;

    public float health = 100;
    public float energy = 100;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraVC = Camera.main;

        // ✅ FIX: Null-Check für Character
        if (transform.childCount > 1)
        {
            Character = transform.GetChild(1).gameObject;
            if (Character != null)
            {
                animator = Character.GetComponent<Animator>();
            }
        }
        
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }

    void Update()
    {


        // ✅ FIX: Null-Checks für UI Elements
        if (healthBar != null)
        {
            healthBar.fillAmount = health / 100f;
        }
        
        if (enegeryBar != null)
        {
            enegeryBar.fillAmount = energy / 100f;
        }

        if (health < 30)
        {
            speed = injuryspeed;
            if (animator != null) // ✅ FIX: Null-Check
            {
                animator.SetBool("isInjured", true);
            }
        }
        else if (health > 30)
        {
            if (animator != null) // ✅ FIX: Null-Check
            {
                animator.SetBool("isInjured", false);
            }
        }

        if (health <= 0)
        {
            SceneManager.LoadScene(2);
        }

   

  
    
     health = Mathf.Clamp(health, 0f, 100f);

#if UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
        {
            float vertical = Input.GetAxis("Vertical");

            dir = transform.forward * speed * vertical;

            

            if (Time.timeScale > 0 && cameraVC != null) // ✅ FIX: Camera Null-Check
            {
                Ray ray = cameraVC.ScreenPointToRay(Input.mousePosition);

                Plane groundPlane = new Plane(Vector3.up, transform.position);

                float hitDist;
                if (groundPlane.Raycast(ray, out hitDist))
                {
                    Vector3 lookPoint = ray.GetPoint(hitDist);
                    Vector3 lookDir = lookPoint - transform.position;
                    lookDir.y = 0f;

                    if (lookDir.sqrMagnitude > 0.001f)
                        transform.rotation = Quaternion.LookRotation(lookDir);
                }
            }
        }
 
        if (animator != null) // ✅ FIX: Null-Check
        {
            animator.SetFloat("speed", dir.magnitude);
        }

#endif

        if (health <= 0)
        {
            health = 0; // Prevent health from going below 0
        }

#if UNITY_IOS
        {
            if (joystick != null) // ✅ FIX: Joystick Null-Check
            {
                horizontal = joystick.Horizontal;
                vertical = joystick.Vertical;

                Vector3 inputDir = new Vector3(horizontal, 0, vertical);

                if (inputDir.sqrMagnitude > 0.001f)
                {
                    // Spieler bewegt sich in Richtung des Joysticks
                    if (rb != null) // ✅ FIX: Null-Check
                    {
                        rb.linearVelocity = inputDir.normalized * speed;
                    }

                    // Spieler schaut in Bewegungsrichtung
                    transform.rotation = Quaternion.LookRotation(inputDir);
                }
                else
                {
                    if (rb != null) // ✅ FIX: Null-Check
                    {
                        rb.linearVelocity = Vector3.zero;
                    }
                }

                if (animator != null) // ✅ FIX: Null-Check
                {
                    animator.SetFloat("speed", joystick.Horizontal + joystick.Vertical);
                }
            }
        }
#endif

        // ✅ FIX: Health Clamping verbessert
       
    }

    // ✅ NEU: Public Methoden für andere Scripts
   

 




}