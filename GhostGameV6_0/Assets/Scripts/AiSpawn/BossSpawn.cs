using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameMode gm;
    public GameObject Player;
    public GameObject Boss;
    public float distanceToPlayer;

    private bool notSpawnAgain;
    void Start()
    {
        if(gm == null)
        {
            gm = GameObject.Find("GameMode").GetComponent<GameMode>();
        }
        if(Player == null)
        {
            Player = GameObject.Find("Player");
        }
        if(Boss == null)
        {
            Boss = Resources.Load<GameObject>("GhostBoss");
        }
    }
    void Update()
    {
        distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);

        if(distanceToPlayer < 15f && notSpawnAgain == false)
        {
            Instantiate(Boss, transform.position, Quaternion.identity);
            notSpawnAgain = true;
            gm.BossDefeated = false;
        }
    }
}
