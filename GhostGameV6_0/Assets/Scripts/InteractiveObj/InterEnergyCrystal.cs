using UnityEngine;

public class InterEnergyCrystal : MonoBehaviour
{

    public float howmuchHealth = 25;
    public float howmuchEnergy = 25;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            pc.energy += howmuchEnergy;
            pc.health += howmuchHealth;
            Destroy(gameObject);
        }
    }
}
