using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WaterCollider : MonoBehaviour
{
    public PlayerController playerController;

 

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerController.health = 0;
        }
    }

    
}