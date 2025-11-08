using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class interblock : MonoBehaviour
{

    public GameObject player;
    private PlayerInventory playerInventory;



    public GameObject button1;

    private GameObject[] Portals;






    public string itemName;
    public int itemNumber;


    void Awake()
    {

        playerInventory = player.GetComponent<PlayerInventory>();

        Portals = new GameObject[6];



        for (int i = 0; i < Portals.Length; i++)
        {
            Portals[i] = GameObject.Find("Portal blue" + i);
        }


    }

    public void OnDoorOpen()
    {
        bool keyFound = false;
        string searchKey = itemName + " " + itemNumber;

        if (playerInventory.inventoryListKey.Contains(searchKey))
        {
            keyFound = true;
        }

        if (keyFound)
        {
            // ✅ ALTERNATIVE: Root GameObject finden
            Transform rootTransform = transform.root;
            rootTransform.gameObject.SetActive(false);

            Debug.Log($"✅ Tür geöffnet mit Schlüssel: {searchKey}");
        }
        else
        {
            Debug.Log($"❌ Schlüssel nicht gefunden: {searchKey}");
        }

    }
}


