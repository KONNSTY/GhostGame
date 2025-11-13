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

    public float damageToPlayer = 5f;

    public GameObject ShockEffect;

    public bool shockenable = false;

    private GameObject InterEnegeryCrystal;

    public bool isDestroyPossibleForai = false;

    public Vector3 lastTarget;

    public Transform PlayerZeroPos;

    [HideInInspector] public bool hasdoneAction = false;

    [HideInInspector] public bool doJustOneTime = true;

    public float health = 100;

    [HideInInspector] public float distancetoTheplayer;

    [HideInInspector] public float distanceToPlayer;

    public bool isAiAttacked = false;

    public enum AiState { MoveToPlayer, GoBack, WarpToPlayer, Die, AttackPlayer }
    public AiState state;

    [HideInInspector] public bool shouldGoback = false;

    public float speed = 7f;

    private bool cantDieAgain;

    public bool isAttacking;

    public Canvas EnemyCanvas;

    public Image healthBar;

    public Vector3 sphereGoingTo;

    public Collider aiCollider;

    [HideInInspector] public bool shouldThePlayerBeAtacked = false;

    [HideInInspector] public bool AiShouldEscape = false;

    public WeaponInLight weaponInLight;

    // ✅ FIX: Damage Cooldown hinzufügen
    private float lastDamageTime = 2f;
    private float damageCooldown = 1f;

    // ✅ FIX: Coroutine-Control Flags
    private bool isDelayRunning = false;
    private bool isWaitRunning = false;
    private bool isAfterAttackRunning = false;

    public GameMode gameMode;

    void Start()
    {
        PlayerZeroPos = transform;
        isAttacking = false;
        health = 100;

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
        Debug.Log(state + " : Current State of " + gameObject.name);

        if (healthBar != null)
        {
            healthBar.fillAmount = health / 100f;
        }

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = speed;
        }

        if (health <= 10 && cantDieAgain == false)
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
                    if (shouldThePlayerBeAtacked == false)
                    {
                        navMeshAgent.stoppingDistance = 3f;
                        navMeshAgent.SetDestination(player.transform.position);
                        
                        // ✅ FIX: Coroutine nur einmal starten
                        if (!isDelayRunning)
                        {
                            StartCoroutine(DelayForNextMove(1.5f));
                            isDelayRunning = true;
                        }
                    }
                    else
                    {
                        navMeshAgent.isStopped = false;
                        navMeshAgent.stoppingDistance = 0.5f;
                        navMeshAgent.SetDestination(player.transform.position);

                        if (navMeshAgent.remainingDistance < 0.5f)
                        {
                            state = AiState.GoBack;
                        }
                    }
                }
                if (navMeshAgent.isStopped == true)
                {
                    state = AiState.GoBack;
                    break;
                }
                break;

            case AiState.GoBack:
                shouldThePlayerBeAtacked = false;
                navMeshAgent.speed = 6f;
                navMeshAgent.isStopped = false;
                
                if (shouldGoback == false)
                {
                    speed = 2f;
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
                
                // ✅ FIX: Coroutine nur einmal starten
                if (!isWaitRunning)
                {
                    StartCoroutine(WaitTilNextAttack(0.5f));
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
                
                if (Time.time >= lastDamageTime + damageCooldown)
                {
                    if (pcontroller != null)
                    {
                        pcontroller.health -= damageToPlayer;
                        lastDamageTime = Time.time;
                        Debug.Log($"💥 AI Angriff! Player Health: {pcontroller.health}");
                    }
                }
                
                // ✅ FIX: Coroutine nur einmal starten
                if (!isAfterAttackRunning)
                {
                    StartCoroutine(AfterAttack(0.7f));
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

    // ✅ FIX: Die() Methode korrigiert
    void Die()
    {
        cantDieAgain = true;
        isDead = true;
        state = AiState.Die;
        
        if (aiCollider != null)
        {
            aiCollider.enabled = false;
        }
        
        if (InterEnegeryCrystal != null && player != null)
        {
            Instantiate(InterEnegeryCrystal, transform.position, Quaternion.identity);
        }
        
        if (weaponInLight != null)
        {
            weaponInLight.killCount++;
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

        if (other.gameObject.name == "Player")
        {
            state = AiState.AttackPlayer;
            isAttacking = true;
        }
        else if (other.name.Contains("Spot Light"))
        {
            AiShouldEscape = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = false;
            state = AiState.GoBack;
            Debug.Log("🚪 Player verlässt Trigger");
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