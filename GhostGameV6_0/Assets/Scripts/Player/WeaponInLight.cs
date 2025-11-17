using JetBrains.Annotations;
using TMPro;

using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

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
        player = transform.parent.gameObject; // Assuming this script is attached to a child of the player
        pcontroller = player.GetComponent<PlayerController>();
        lightIntensity = spotLight.intensity; // Store the initial intensity
        canAiBeDamaged = false;
        FXLight.SetActive(false);
    }

    void Update()
    {
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

#if UNITY_IOS
        {
            touch = Input.GetTouch(0);

            if(killCount == 3)
            {
                playerController.Maxhealthitem = 3;
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

        if (isUnableToUsWeapon == false)
        {
            if (Input.GetMouseButton(0))
            {
                if (pcontroller.energy > 10)
                {
                    spotLight.intensity = 300f;
                    FXLight.SetActive(true);

                    pcontroller.energy -= 30f * Time.deltaTime; // Reduce energy while the light is on

                    canAiBeDamaged = true; // Enable damage to AI when the light is on
                    keyUP = false; // Reset keyUP to false when the mouse button is pressed
                }
                else if (pcontroller.energy <= 10)
                {
                    spotLight.intensity = lightIntensity; // Reset to original intensity if energy is too low
                    pcontroller.energy = 0; // Prevent energy from going negative
                    spotLight.color = Color.white;
                    canAiBeDamaged = false;
                    keyUP = true;
                    FXLight.SetActive(false);
                }
            }
        }
        else
        {
            spotLight.intensity = 0f;
            FXLight.SetActive(false);
            canAiBeDamaged = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            spotLight.intensity = lightIntensity;
            FXLight.SetActive(false);
            canAiBeDamaged = false;
            keyUP = true; // Set keyUP to true when the mouse button is released
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Debug für schnellere Fehleranalyse
        Debug.Log($"WeaponInLight OnTriggerEnter: other={other.gameObject.name}, tag={other.gameObject.tag}, canAiBeDamaged={canAiBeDamaged}");

        if (!canAiBeDamaged) return;

        // Erkenne zuerst AiBoss per Komponente (robust gegenüber Tags/Namen)
        AiBoss aiBoss = other.GetComponent<AiBoss>();
        if (aiBoss != null)
        {
            if (aiBoss.navMeshAgent != null)
                aiBoss.navMeshAgent.ResetPath();

            aiBoss.state = AiBoss.AiState.GoBack;
            aiBoss.AiShouldEscape = true;
            aiBoss.shouldGoback = false;
            aiBoss.health -= lightdamage;
            aiBoss.shockenable = true;

            Debug.Log($"AI Boss hit by light. New health: {aiBoss.health}");
            return;
        }

        // Falls kein AiBoss, versuche normale Ai
        AiController ai = other.GetComponent<AiController>();
        if (ai != null)
        {
            if (ai.navMeshAgent != null)
                ai.navMeshAgent.ResetPath();

            ai.state = AiController.AiState.GoBack;
            ai.AiShouldEscape = true;
            ai.shouldGoback = false;
            ai.health -= lightdamage;
            ai.shockenable = true;

            Debug.Log($"AI hit by light. New health: {ai.health}");
            return;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // Debug für schnellere Fehleranalyse
        // (kommentiere out wenn zu spammy)
        // Debug.Log($"WeaponInLight OnTriggerStay: other={other.gameObject.name}, tag={other.gameObject.tag}, canAiBeDamaged={canAiBeDamaged}");

        if (!canAiBeDamaged) return;

        AiBoss aiBoss = other.GetComponent<AiBoss>();
        if (aiBoss != null)
        {
            // kontinuerlicher Schaden (frameunabhängig)
            aiBoss.health -= lightdamage * Time.deltaTime;
            aiBoss.shockenable = true;

            if (aiBoss.state != AiBoss.AiState.GoBack)
            {
                aiBoss.state = AiBoss.AiState.GoBack;
                aiBoss.AiShouldEscape = true;
                aiBoss.shouldGoback = false;
            }

            // gelegentliches Loggen zur Kontrolle
            // Debug.Log($"AI Boss continuous damage: {aiBoss.health}");
            return;
        }

        AiController ai = other.GetComponent<AiController>();
        if (ai != null)
        {
            ai.health -= lightdamage * Time.deltaTime;
            ai.shockenable = true;

            if (ai.state != AiController.AiState.GoBack)
            {
                ai.state = AiController.AiState.GoBack;
                ai.AiShouldEscape = true;
                ai.shouldGoback = false;
            }

            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"WeaponInLight OnTriggerExit: other={other.gameObject.name}, tag={other.gameObject.tag}");

        AiBoss aiBoss = other.GetComponent<AiBoss>();
        if (aiBoss != null)
        {
            aiBoss.AiShouldEscape = false;
            aiBoss.shockenable = false;
            return;
        }

        AiController ai = other.GetComponent<AiController>();
        if (ai != null)
        {
            ai.AiShouldEscape = false;
            ai.shockenable = false;
            return;
        }
    }
}
