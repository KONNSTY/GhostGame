using UnityEngine;

public class BoatTranslate : MonoBehaviour
{
    public float speed;



    private Rigidbody rb;

    public float dir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {

        Vector3 movement = new Vector3(dir,0,0) * speed * Time.deltaTime;
        Vector3 newPosition = rb.position + movement;

        rb.MovePosition(newPosition);
    }
}