using System.Collections;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AiBoss : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    [HideInInspector] public Vector3 playertarget;
    [HideInInspector] public bool isDead = false;

    public GameObject player;
    public PlayerController pcontroller;
    private bool shouldDestoryGameObject = false;

    public GameObject Ghost;
    public Animator GhostAnimator;

    public float damageToPlayer = 5f; // ✅ BOSS: 5x mehr Schaden als normale AI (war 15f)

    public GameObject ShockEffect;

    public bool shockenable = false;

    private GameObject InterEnegeryCrystal;

    public bool isDestroyPossibleForai = false;

    public Vector3 lastTarget;

    public Transform PlayerZeroPos;

    [HideInInspector] public bool hasdoneAction = false;

    [HideInInspector] public bool doJustOneTime = true;

    public float health = 300f; // ✅ BOSS: 3x mehr Health als normale AI

    [HideInInspector] public float distancetoTheplayer;

    [HideInInspector] public float distanceToPlayer;

    public bool isAiAttacked = false;

    public enum AiState { MoveToPlayer, GoBack, WarpToPlayer, Die, AttackPlayer }
    public AiState state;

    [HideInInspector] public bool shouldGoback = false;

    public float speed = 12f; // ✅ BOSS: Noch schneller als normale AI (war 9f)

    private bool cantDieAgain;

    public bool isAttacking;

    public Canvas EnemyCanvas;

    public Image healthBar;

    public Vector3 sphereGoingTo;

    public Collider aiCollider;

    [HideInInspector] public bool shouldThePlayerBeAtacked = false;

    [HideInInspector] public bool AiShouldEscape = false;

    public WeaponInLight weaponInLight;

    // ✅ BOSS: Schnellerer Angriff als normale AI
    private float lastDamageTime = 0f;
    private float damageCooldown = 1f; // ✅ BOSS: 2x schnellerer Angriff als normale AI (war 0.5f)

    // ✅ FIX: Coroutine-Control Flags
    private bool isDelayRunning = false;
    private bool isWaitRunning = false;
    private bool isAfterAttackRunning = false;
    
    public bool isCollideWithPlayer = false; // ✅ FIX: Track collision with player

    public GameMode gameMode;

    void Start()
    {
        PlayerZeroPos = transform;
        isAttacking = false;
        health = 300f; // ✅ BOSS: 3x mehr Health

        navMeshAgent = GetComponent<NavMeshAgent>();
        EnemyCanvas = GetComponentInChildren<Canvas>();

        if(gameMode == null)
        {
            gameMode = GameObject.Find("GameMode").GetComponent<GameMode>();
        }

        if (EnemyCanvas != null)
        {
            healthBar = EnemyCanvas.GetComponentInChildren<Image>();
        }

        if (player == null)
        {
            player = GameObject.FindAnyObjectByType<PlayerController>().gameObject;
            //player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            pcontroller = player.GetComponent<PlayerController>();
            weaponInLight = player.GetComponentInChildren<WeaponInLight>();
        }

        if (GhostAnimator == null)
        {
            GhostAnimator = GetComponentInChildren<Animator>();
        }

        InterEnegeryCrystal = Resources.Load("InterEnegeryCrystal") as GameObject;
        if (InterEnegeryCrystal == null)
        {
            InterEnegeryCrystal = Resources.Load("InterEngryCrystal") as GameObject;
        }

        aiCollider = GetComponent<Collider>();

        state = AiState.MoveToPlayer;
        cantDieAgain = false;

        if (navMeshAgent != null)
        {
            navMeshAgent.stoppingDistance = 0f;
        }
    }

    void Update()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = health / 300f; // ✅ BOSS: Health Bar für 300 HP
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = speed;
        }

        if (health <= 30 && cantDieAgain == false) // ✅ BOSS: Stirbt bei 30 HP (10% von 300)
        {
            Die(); // ✅ FIX: Verwende Die() Methode
            return;
        }

        if (shouldDestoryGameObject == true)
            Destroy(gameObject, 2f);

        if (isDead && state == AiState.Die) return;

        if (shockenable == true)
        {
            if (ShockEffect != null)
            {
                ShockEffect.SetActive(true);
            }
            shockenable = false;
        }
        else
        {
            if (ShockEffect != null)
            {
                ShockEffect.SetActive(false);
            }
        }

        if (player != null)
        {
            playertarget = player.transform.position;
        }

        if (GhostAnimator != null)
        {
            GhostAnimator.SetFloat("Speed", speed);
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = true;
        }

        if (player != null)
        {
            distancetoTheplayer = Vector3.Distance(player.transform.position, lastTarget);
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        }

        if (navMeshAgent == null) return;

        switch (state)
        {
            case AiState.MoveToPlayer:
                navMeshAgent.speed = speed;

                if (navMeshAgent.isStopped == false && player != null)
                {
                    // ✅ BOSS: Gehe direkt zum Spieler ohne Verzögerung oder Zurücklaufen
                    navMeshAgent.stoppingDistance = 1.5f; // Näher an den Spieler
                    navMeshAgent.SetDestination(player.transform.position);
                    
                    // ✅ BOSS: Kein automatisches GoBack beim Erreichen des Spielers
                    // Der Boss bleibt beim Spieler und wartet auf Trigger-Kontakt
                }
                
                // ✅ BOSS: Entferne automatisches GoBack bei isStopped
                // Boss soll nur zurücklaufen nach Angriff oder bei Light-Flucht
                break;

            case AiState.GoBack:
                shouldThePlayerBeAtacked = false;
                navMeshAgent.speed = 15f; // ✅ BOSS: Sehr schnelle Flucht (war 10f)
                navMeshAgent.isStopped = false;
                
                if (shouldGoback == false)
                {
                    speed = 6f; // ✅ BOSS: Schnellere Recovery (war 4f)
                    Vector3 dir = transform.position - playertarget;
                    dir.y = 0f;
                    dir = dir.normalized;
                    Vector3 randomSphere = UnityEngine.Random.insideUnitSphere * 6f;
                    lastTarget = transform.position + dir * 6f;
                    Vector3 targetPosition = lastTarget + randomSphere;

                    navMeshAgent.SetDestination(targetPosition);
                    shouldGoback = true;
                }
                
                AiShouldEscape = false;
                
                // ✅ BOSS: Schnellere Wartezeit als normale AI
                if (!isWaitRunning)
                {
                    StartCoroutine(WaitTilNextAttack(0.3f)); // ✅ BOSS: Viel schneller als normale AI (war 0.5f)
                    isWaitRunning = true;
                }
                break;

            case AiState.WarpToPlayer:
                if (player != null)
                {
                    Vector3 warpPos = player.transform.position + UnityEngine.Random.insideUnitSphere * 5f;
                    warpPos.y = transform.position.y;
                    navMeshAgent.Warp(warpPos);
                }
                state = AiState.MoveToPlayer;
                break;

            case AiState.Die:
                // ✅ FIX: Die State implementiert
                gameMode.BossDefeated = true;

                if (GhostAnimator != null)
                {
                    GhostAnimator.SetBool("Dead", true);
                }
                
                // Destroy nach 2 Sekunden
                if (!shouldDestoryGameObject)
                {
                    shouldDestoryGameObject = true;
                    Destroy(gameObject, 2f);
                }
                break;

            case AiState.AttackPlayer:
                navMeshAgent.isStopped = true;
                
                if (isCollideWithPlayer == true) // ✅ FIX: Nur Schaden wenn im Trigger
                {
                    if (Time.time >= lastDamageTime + damageCooldown)
                    {
                        if (pcontroller != null)
                        {
                            pcontroller.health -= damageToPlayer;
                            lastDamageTime = Time.time;
                            Debug.Log($"💥 BOSS Angriff! Player Health: {pcontroller.health}");
                        }
                    }
                }
                
                // ✅ BOSS: Kürzere Attack-Animation
                if (!isAfterAttackRunning)
                {
                    StartCoroutine(AfterAttack(0.4f)); // ✅ BOSS: Schnellere Animation (war 0.7f)
                    isAfterAttackRunning = true;
                }
                break;
        }

        if (AiShouldEscape == true)
        {
            navMeshAgent.ResetPath();
            StopAllCoroutines();
            
            // ✅ FIX: Flags zurücksetzen
            ResetCoroutineFlags();
            
            state = AiState.GoBack;
            AiShouldEscape = false;
        }
    }

    // ✅ BOSS: Erweiterte Die() Methode mit Boss-Features
    void Die()
    {
        cantDieAgain = true;
        isDead = true;
        state = AiState.Die;
        
        if (aiCollider != null)
        {
            aiCollider.enabled = false;
        }
        
        // ✅ BOSS: Mehrere Kristalle droppen (3-5 Stück)
        if (InterEnegeryCrystal != null && player != null)
        {
            int crystalCount = UnityEngine.Random.Range(3, 6); // 3-5 Kristalle
            for (int i = 0; i < crystalCount; i++)
            {
                Vector3 crystalPos = transform.position + UnityEngine.Random.insideUnitSphere * 2f;
                crystalPos.y = transform.position.y + 1f; // Leicht erhöht spawnen
                Instantiate(InterEnegeryCrystal, crystalPos, Quaternion.identity);
            }
            Debug.Log($"💎 Boss droppt {crystalCount} Kristalle!");
        }
        
        // ✅ BOSS: Mehr Kill Count (3 statt 1)
        if (weaponInLight != null)
        {
            weaponInLight.killCount += 3; // Boss zählt als 3 Kills
            Debug.Log($"🏆 Boss besiegt! Kill Count: +3");
        }
        
        shouldDestoryGameObject = true;
        
        // NavMesh nach 1 Sekunde deaktivieren
        Invoke("DisableNavMesh", 1f);
    }

    // ✅ NEU: Coroutine Flags zurücksetzen
    void ResetCoroutineFlags()
    {
        isDelayRunning = false;
        isWaitRunning = false;
        isAfterAttackRunning = false;
    }

    void DisableNavMesh()
    {
        if (navMeshAgent != null && navMeshAgent.enabled)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.enabled = false;
        }
    }

    IEnumerator WaitTilNextAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        if (!isDead)
        {
            state = AiState.MoveToPlayer;
            shouldGoback = false; // ✅ FIX: Reset shouldGoback
        }
        
        isWaitRunning = false; // ✅ FIX: Flag zurücksetzen
    }

    IEnumerator DelayForNextMove(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        if (!isDead)
        {
            shouldThePlayerBeAtacked = true;
        }
        
        isDelayRunning = false; // ✅ FIX: Flag zurücksetzen
    }

    IEnumerator AfterAttack(float waitTime)
    {
        if (GhostAnimator != null)
        {
            GhostAnimator.SetBool("Attack", true);
        }

        yield return new WaitForSeconds(waitTime);

        if (GhostAnimator != null)
        {
            GhostAnimator.SetBool("Attack", false);
        }

        if (!isDead)
        {
            state = AiState.GoBack;
        }
        
        isAfterAttackRunning = false; // ✅ FIX: Flag zurücksetzen
    }

    void OnTriggerStay(Collider other)
    {
        if (isDead) return;

        isCollideWithPlayer = true; // ✅ FIX: Set collision flag

        if (other.CompareTag("player")) // ✅ FIX: Use lowercase player tag
        {
            state = AiState.AttackPlayer;
            isAttacking = true;
            Debug.Log($"🎯 BOSS {gameObject.name} erkannt Spieler: {other.name}");
        }
        else if (other.CompareTag("weapon")) // ✅ FIX: Verwende weapon Tag statt Spot Light name
        {
            AiShouldEscape = true;
            Debug.Log($"💡 BOSS {gameObject.name} flüchtet vor Licht!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        isCollideWithPlayer = false; // ✅ FIX: Clear collision flag
        
        if (other.CompareTag("Player"))
        {
            isAttacking = false;
            // ✅ BOSS: Gehe direkt wieder zum Spieler statt zurückzulaufen
            state = AiState.MoveToPlayer; 
            Debug.Log("🚪 Boss: Player verlässt Trigger - verfolge weiter!"); 
        }

        if (navMeshAgent != null && !isDead)
        {
            navMeshAgent.isStopped = false;
        }

        if (GhostAnimator != null)
        {
            GhostAnimator.SetBool("Attack", false);
        }

        shockenable = false;
    }
}