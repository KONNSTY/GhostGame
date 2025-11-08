using UnityEngine;

public class CircleWall : MonoBehaviour
{
    public float radius;

    public GameObject playerP;



    [SerializeField] private float distance;

    Transform zeroPos;

    void Start()
    {
        zeroPos = transform;

        if (playerP == null)
        {
            playerP = GameObject.Find("Player");
        }



    }
    void Update()
    {
        
        if (playerP != null)
        {
            distance = Vector3.Distance(zeroPos.position, playerP.transform.position);
        }

        if(distance > radius)
        {
            playerP.transform.position = zeroPos.position + (playerP.transform.position - zeroPos.position).normalized * radius;
        }
    }
}
