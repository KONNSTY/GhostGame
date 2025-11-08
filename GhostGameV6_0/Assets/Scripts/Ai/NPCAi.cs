using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class NPCAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target1;
    public Transform target2;

    public float aSpeed = 1.5f;

  

    private bool dontDoAgain;
    private bool dontDoAgain2;

    public enum AiMovement
    {
        FirstGaol,
        SecondGoal,
    }

    public AiMovement state = AiMovement.FirstGaol;


    void Awake()
    {
        target2.position = transform.position;
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Set target2 position to the empty target's position)


        agent.stoppingDistance = 0;
        dontDoAgain = false;
        dontDoAgain2 = false;
        agent.speed = aSpeed;
    }

    void Update()
    {
        switch (state)
        {
            case AiMovement.FirstGaol:
                agent.isStopped = false;
                agent.SetDestination(target1.position);

                // Bessere Ziel-Erreichung-Prüfung
                if (!agent.pathPending && agent.remainingDistance < 0.5f && !agent.hasPath)
                {
                    if (dontDoAgain == false)
                    {
                        agent.isStopped = true;
                        StartCoroutine(WaitForXSecondsTarget1(1));
                        dontDoAgain = true;
                    }
                }
                break;

            case AiMovement.SecondGoal:
                agent.isStopped = false;
                agent.SetDestination(target2.position);
                
                // Bessere Ziel-Erreichung-Prüfung
                if (!agent.pathPending && agent.remainingDistance < 0.5f && !agent.hasPath)
                {
                    if (dontDoAgain2 == false)
                    {
                        agent.isStopped = true;
                        StartCoroutine(WaitForXSecondsTarget2(1));
                        dontDoAgain2 = true;
                    }
                }
                break;
        }
    }

    public IEnumerator WaitForXSecondsTarget1(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        state = AiMovement.SecondGoal;
        dontDoAgain = false;
        // StopAllCoroutines(); // ❌ Entfernt - stoppt die eigene Coroutine!
    }
    
    public IEnumerator WaitForXSecondsTarget2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        state = AiMovement.FirstGaol;
        dontDoAgain2 = false;
        // StopAllCoroutines(); // ❌ Entfernt - stoppt die eigene Coroutine!
    }
}
