using UnityEngine;

public class WeaponInLight : MonoBehaviour
{
    public Light spotLight;

    public int killCount = 0;

    public GameObject player;

  
    public PlayerController pcontroller;

    public bool canReload = false; // Flag to control reloading

    public bool keyUP;
    public float lightIntensity; // Set your desired light intensity here

    public float lightdamage = 20;
    // Damage dealt by the light

    public bool canAiBeDamaged = false; // Flag to control if AI can be damaged by light
    [HideInInspector] public bool isTouching = false;
    [HideInInspector] public Touch touch;

    public GameObject FXLight;

    public bool isUnableToUsWeapon = false;



    void Start()
    {
        spotLight = GetComponent<Light>();
        if (transform.parent != null)
            player = transform.parent.gameObject;
        if (player != null)
            pcontroller = player.GetComponent<PlayerController>();
        if (spotLight != null)
            lightIntensity = spotLight.intensity;
        canAiBeDamaged = false;
        SetFxActive(false);
    

       


    }

    void Update()
    {
        if (pcontroller == null || spotLight == null)
            return;

        if (pcontroller.energy == 100)
        {
            canReload = false;
        }
        else if (pcontroller.energy < 100 && keyUP == true)
        {
            canReload = true;
        }
        if (pcontroller.energy > 100)
        {
            pcontroller.energy = 100; // Clamp energy to a maximum of 100
        }

        if (canReload == true)
        {
            pcontroller.energy += 15f * Time.deltaTime; // Increase energy by 10
        }

        /*

        Touch
        
        */
#if UNITY_IOS
        if (Input.touchCount > 0)
        {
        touch = Input.GetTouch(0);

    if(killCount == 3)
    {
        pcontroller.Maxhealthitem = 3;
    }

        if (touch.phase == TouchPhase.Began)
        {
            isTouching = true;

            if (pcontroller.energy > 10)
            {
                spotLight.intensity = 300f;
                spotLight.color = Color.blue;
                pcontroller.energy -= 30f; // Reduce energy when the light is turned on
                canAiBeDamaged = true; // Enable damage to AI when the light is on
                keyUP = false; // Reset keyUP to false when the touch begins
            }
            else if (pcontroller.energy <= 10)
            {
                spotLight.intensity = lightIntensity; // Reset to original intensity if energy is too low
                pcontroller.energy = 0; // Prevent energy from going negative
                spotLight.color = Color.white;
                canAiBeDamaged = false; // Disable damage to AI when the light is off
                keyUP = true;
            }
        }

       
        if(isTouching == true)
        {
            if (pcontroller.energy > 10)
            {
                spotLight.intensity = 300f;
                spotLight.color = Color.blue;
                pcontroller.energy -= 30f; // Reduce energy when the light is turned on
                canAiBeDamaged = true; // Enable damage to AI when the light is on
                keyUP = false; // Reset keyUP to false when the touch begins
            }
            else if (pcontroller.energy <= 10)
            {
                spotLight.intensity = lightIntensity; // Reset to original intensity if energy is too low
                pcontroller.energy = 0; // Prevent energy from going negative
                spotLight.color = Color.white;
                canAiBeDamaged = false; // Disable damage to AI when the light is off
                keyUP = true;
            }
        }
        
          if (touch.phase == TouchPhase.Ended)
        {
            isTouching = false;
            spotLight.intensity = lightIntensity;
            spotLight.color = Color.white;
            canAiBeDamaged = false;
            keyUP = true; // Set keyUP to true when the mouse button is released
        }

        }
#endif

        /*

Mouse

*/

        if (isUnableToUsWeapon == false)
        {



            if (Input.GetMouseButton(0))
            {
                if (pcontroller.energy > 10)
                {
                    spotLight.intensity = 300f;
                    SetFxActive(true);
                    

                    pcontroller.energy -= 30f * Time.deltaTime; // Reduce energy while the light is on

                    canAiBeDamaged = true; // Enable damage to AI when the light is on
                    keyUP = false; // Reset keyUP to false when the mouse button is pressed
                }
                else if (pcontroller.energy <= 10)
                {
                    spotLight.intensity = lightIntensity; // Reset to original intensity if energy is too low
                    pcontroller.energy = 0; // Prevent energy from going negative
                                            // Reduce energy while the light is on
                    spotLight.color = Color.white;
                    canAiBeDamaged = false; // Enable damage to AI when the light is on
                    keyUP = true;
                    SetFxActive(false);
                }
            }
        } else if (isUnableToUsWeapon == true)
        {
            spotLight.intensity = 0f;
            SetFxActive(false);
            canAiBeDamaged = false;
        }

        if (Input.GetMouseButtonUp(0))
            {
                spotLight.intensity = lightIntensity;
                SetFxActive(false);
             

                canAiBeDamaged = false;
                keyUP = true; // Set keyUP to true when the mouse button is released
            }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ai") && canAiBeDamaged == true )
        {
            AiController ai = other.GetComponent<AiController>();
            
            if (ai != null)
            {
                // AI zum Rückzug zwingen
                ai.state = AiController.AiState.GoBack;
                ai.AiShouldEscape = true;
                ai.shouldGoback = false; // Reset für neue GoBack-Bewegung
                
                // AI stoppen und Schaden zufügen
                if (ai.navMeshAgent != null)
                    ai.navMeshAgent.ResetPath();
                ai.health -= lightdamage;
                ai.shockenable = true; // Activate shock effect on AI
                
                Debug.Log("AI forced to GoBack state by light!");
            }
        }
        else if (other.gameObject.name == "GhostBoss" && canAiBeDamaged == true)
        {
            AiBoss aiBoss = other.GetComponent<AiBoss>();
            
            if (aiBoss != null)
            {
                // AI Boss zum Rückzug zwingen
                aiBoss.state = AiBoss.AiState.GoBack;
                aiBoss.AiShouldEscape = true;
                aiBoss.shouldGoback = false; // Reset für neue GoBack-Bewegung
                
                // AI Boss stoppen und Schaden zufügen
                if (aiBoss.navMeshAgent != null)
                    aiBoss.navMeshAgent.ResetPath();
                aiBoss.health -= lightdamage;
                aiBoss.shockenable = true; // Activate shock effect on AI Boss
                
                Debug.Log("AI Boss forced to GoBack state by light!");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ai") && canAiBeDamaged == true)
        {
            AiController ai = other.GetComponent<AiController>();
            
            if (ai != null)
            {
                // Kontinuierlichen Schaden zufügen
                ai.health -= lightdamage * Time.deltaTime;
                ai.shockenable = true;
                
                // AI im GoBack-State halten
                if (ai.state != AiController.AiState.GoBack)
                {
                    ai.state = AiController.AiState.GoBack;
                    ai.AiShouldEscape = true;
                    ai.shouldGoback = false; // Reset für neue GoBack-Bewegung
                    Debug.Log("AI kept in GoBack state by continuous light!");
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ai"))
        {
            AiController ai = other.GetComponent<AiController>();
            
            if (ai != null)
            {
                // AI kann wieder normal agieren wenn sie das Licht verlässt
                ai.AiShouldEscape = false;
                ai.shockenable = false;
                Debug.Log("AI left light area");
            }
        }
    }

    void SetFxActive(bool active)
    {
        if (FXLight != null)
            FXLight.SetActive(active);
    }
}
