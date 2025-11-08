using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Photon.Realtime;

public class NPcFollowScript : MonoBehaviour
{
    public bool isSebastianFollowing;
    public bool isLinaFollowing = false;
    public bool isSisterFollowing = false;

    public Vector3 orginPos;

    public GameObject player;

    public PlayerController playerController;

    public enum chars
    {
        Sebastian,
        Lina,

        Sister,


    }

    public chars characterE;

    public NavMeshAgent agent;

    public Animator sebAnimator;
    public Animator linaAnimator;
    public Animator sisterAnimator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        orginPos = transform.position;
       
            
                sebAnimator = transform.GetChild(0).GetComponent<Animator>();
            
      
                linaAnimator = transform.GetChild(0).GetComponent<Animator>();
               
         
                sisterAnimator = transform.GetChild(0).GetComponent<Animator>();
            
        
    
    agent.speed = 2f;
    }

    void Update()
    {
        switch (characterE)
        {
            case chars.Sebastian:
                if (isSebastianFollowing == true)
                {
                    sebAnimator.SetFloat("speed", 2f);
                    agent.isStopped = false;
                    agent.stoppingDistance = 4f;
                    agent.speed = 2f;
                    agent.SetDestination(player.transform.position);

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        sebAnimator.SetFloat("speed", 0f);
                    }


                }
                else
                {
                      agent.isStopped = false;
                    agent.stoppingDistance = 0;
                    agent.SetDestination(orginPos);
                  
                }
                break;

            case chars.Lina:
                if (isLinaFollowing == true)
                {
                    
                    sebAnimator.SetFloat("speed", agent.speed);
                    agent.isStopped = false;
                    
                    agent.stoppingDistance = 5f;
                    agent.speed = 5f;
                    agent.SetDestination(player.transform.position);

                    if(Vector3.Distance(transform.position, player.transform.position) > 40)
                    {
                        agent.Warp(player.transform.position + (transform.position - player.transform.position).normalized * 10f);
                    }

                    if(agent.remainingDistance <= agent.stoppingDistance)
                    {
                        linaAnimator.SetFloat("speed", 0);
                    }
                }
                else
                {
                    agent.isStopped = true;
                    linaAnimator.SetFloat("speed", 0);
                }
                break;

            case chars.Sister:
                if (isSisterFollowing == true)
                {
                   
                    sisterAnimator.SetBool("Idle", true);
                    sisterAnimator.SetFloat("speed", agent.speed);
                    agent.isStopped = false;
                    agent.stoppingDistance = 6f;
                    agent.speed = 5f;
                    agent.SetDestination(player.transform.position);

                     if(Vector3.Distance(transform.position, player.transform.position) > 40)
                    {
                        agent.Warp(player.transform.position + (transform.position - player.transform.position).normalized * 10f);
                    }

                    if(agent.remainingDistance <= agent.stoppingDistance)
                    {
                     sisterAnimator.SetFloat("speed", 0);
                    }
                }
                else
                {
                    agent.isStopped = true;
                 sisterAnimator.SetFloat("speed", 0);
                }
                break;
        }
    }
}
