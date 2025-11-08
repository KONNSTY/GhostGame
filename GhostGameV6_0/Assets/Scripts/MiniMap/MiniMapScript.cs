using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    public Transform player;
  

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, 40, player.transform.position.z);
    }
}
