using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    public PlayerController playerController;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player" && !other.CompareTag("player"))
            return;

        PlayerController controller = playerController != null
            ? playerController
            : other.GetComponent<PlayerController>();
        if (controller != null)
            controller.health = 0f;
    }

    
}